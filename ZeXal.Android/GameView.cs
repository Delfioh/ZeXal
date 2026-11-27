using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using ZeXal.DMG;
using GBButton = ZeXal.DMG.Button;

namespace ZeXal.Android
{
    //Draws the Game Boy display and the on-screen controls, and turns touches into button presses.
    //Portrait: display on top, controls below. Landscape: display in the middle, controls on the sides.
    class GameView : View
    {
        private const GBButton AllButtons = GBButton.Right | GBButton.Left | GBButton.Up | GBButton.Down |
                                            GBButton.A | GBButton.B | GBButton.Select | GBButton.Start;

        private readonly Bitmap screen = Bitmap.CreateBitmap(PPU.ScreenWidth, PPU.ScreenHeight, Bitmap.Config.Argb8888);
        private readonly int[] pixels;
        private readonly object pixels_lock;

        private readonly Paint pixel_paint = new Paint { FilterBitmap = false };
        private readonly Paint fill = new Paint(PaintFlags.AntiAlias);
        private readonly Paint text = new Paint(PaintFlags.AntiAlias) { TextAlign = Paint.Align.Center };

        private readonly Dictionary<int, GBButton> touches = new Dictionary<int, GBButton>();
        private GBButton pressed = GBButton.None;
        private bool menu_touched;

        //Layout
        private Rect screen_rect = new Rect();
        private RectF bezel_rect = new RectF();
        private float dpad_x, dpad_y, dpad_r;
        private float a_x, a_y, b_x, b_y, button_r;
        private RectF select_rect = new RectF(), start_rect = new RectF(), menu_rect = new RectF();
        private int inset_left, inset_top, inset_right, inset_bottom;

        public event Action<GBButton> ButtonsChanged;
        public event Action MenuRequested;

        public string Message { get; set; }

        public GameView(Context context, int[] pixels, object pixels_lock) : base(context)
        {
            this.pixels = pixels;
            this.pixels_lock = pixels_lock;
            Focusable = true;
            FocusableInTouchMode = true;
        }

        #region Layout

        public override WindowInsets OnApplyWindowInsets(WindowInsets insets)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                Insets i = insets.GetInsetsIgnoringVisibility(WindowInsets.Type.SystemBars() | WindowInsets.Type.DisplayCutout());
                inset_left = i.Left; inset_top = i.Top; inset_right = i.Right; inset_bottom = i.Bottom;
            }
            else
            {
                inset_left = insets.SystemWindowInsetLeft; inset_top = insets.SystemWindowInsetTop;
                inset_right = insets.SystemWindowInsetRight; inset_bottom = insets.SystemWindowInsetBottom;
            }
            DoLayout(Width, Height);
            Invalidate();
            return insets;
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            DoLayout(w, h);
        }

        private static float Dp(View v, float dp)
        {
            return dp * v.Resources.DisplayMetrics.Density;
        }

        private void DoLayout(int full_w, int full_h)
        {
            if (full_w == 0 || full_h == 0) return;

            float left = inset_left, top = inset_top;
            float w = full_w - inset_left - inset_right;
            float h = full_h - inset_top - inset_bottom;
            float margin = Dp(this, 12);

            if (h >= w)
            {
                //Portrait
                float scale = (w - 2 * margin) / PPU.ScreenWidth;
                scale = Math.Min(scale, (h * 0.55f) / PPU.ScreenHeight);
                if (scale >= 2) scale = (float)Math.Floor(scale);
                int sw = (int)(PPU.ScreenWidth * scale), sh = (int)(PPU.ScreenHeight * scale);
                int sx = (int)(left + (w - sw) / 2), sy = (int)(top + margin * 2);
                screen_rect.Set(sx, sy, sx + sw, sy + sh);

                float controls_top = sy + sh + margin * 2;
                float controls_h = top + h - controls_top;
                dpad_r = Math.Min(w * 0.19f, controls_h * 0.3f);
                dpad_x = left + w * 0.27f;
                dpad_y = controls_top + controls_h * 0.4f;
                button_r = dpad_r * 0.42f;
                a_x = left + w * 0.84f; a_y = dpad_y - button_r * 0.7f;
                b_x = left + w * 0.62f; b_y = dpad_y + button_r * 0.7f;

                float pill_w = Dp(this, 64), pill_h = Dp(this, 26);
                float pills_y = Math.Min(controls_top + controls_h * 0.82f, top + h - pill_h - margin);
                float cx = left + w / 2;
                select_rect.Set(cx - pill_w * 1.15f - pill_w / 2, pills_y, cx - pill_w * 1.15f + pill_w / 2, pills_y + pill_h);
                start_rect.Set(cx - pill_w / 2, pills_y, cx + pill_w / 2, pills_y + pill_h);
                menu_rect.Set(cx + pill_w * 1.15f - pill_w / 2, pills_y, cx + pill_w * 1.15f + pill_w / 2, pills_y + pill_h);
            }
            else
            {
                //Landscape
                float scale = (h - 2 * margin) / PPU.ScreenHeight;
                scale = Math.Min(scale, (w * 0.56f) / PPU.ScreenWidth);
                if (scale >= 2) scale = (float)Math.Floor(scale);
                int sw = (int)(PPU.ScreenWidth * scale), sh = (int)(PPU.ScreenHeight * scale);
                int sx = (int)(left + (w - sw) / 2), sy = (int)(top + (h - sh) / 2);
                screen_rect.Set(sx, sy, sx + sw, sy + sh);

                float side_w = (w - sw) / 2;
                dpad_r = Math.Min(side_w * 0.36f, h * 0.2f);
                dpad_x = left + side_w / 2;
                dpad_y = top + h * 0.5f;
                button_r = dpad_r * 0.42f;
                float rx = left + w - side_w / 2;
                a_x = rx + button_r * 1.15f; a_y = dpad_y - button_r * 0.7f;
                b_x = rx - button_r * 1.15f; b_y = dpad_y + button_r * 0.7f;

                float pill_w = Math.Min(Dp(this, 64), side_w * 0.42f), pill_h = Dp(this, 26);
                float py = top + h - pill_h - margin;
                select_rect.Set(dpad_x - pill_w / 2, py, dpad_x + pill_w / 2, py + pill_h);
                start_rect.Set(rx - pill_w / 2, py, rx + pill_w / 2, py + pill_h);
                menu_rect.Set(rx - pill_w / 2, top + margin, rx + pill_w / 2, top + margin + pill_h);
            }

            float bezel = Dp(this, 10);
            bezel_rect.Set(screen_rect.Left - bezel, screen_rect.Top - bezel, screen_rect.Right + bezel, screen_rect.Bottom + bezel);
        }

        #endregion

        #region Drawing

        protected override void OnDraw(Canvas canvas)
        {
            canvas.DrawColor(Color.Rgb(0xC8, 0xC8, 0xC2));

            fill.Color = Color.Rgb(0x5A, 0x5A, 0x6E);
            canvas.DrawRoundRect(bezel_rect, Dp(this, 8), Dp(this, 8), fill);

            lock (pixels_lock) screen.SetPixels(pixels, 0, PPU.ScreenWidth, 0, 0, PPU.ScreenWidth, PPU.ScreenHeight);
            canvas.DrawBitmap(screen, null, screen_rect, pixel_paint);

            if (!String.IsNullOrEmpty(Message))
            {
                text.Color = Color.Rgb(0x0F, 0x38, 0x0F);
                text.TextSize = Dp(this, 16);
                float y = screen_rect.CenterY();
                foreach (string line in Message.Split('\n'))
                {
                    canvas.DrawText(line, screen_rect.CenterX(), y, text);
                    y += text.TextSize * 1.3f;
                }
            }

            DrawDPad(canvas);
            DrawRoundButton(canvas, a_x, a_y, "A", (pressed & GBButton.A) != 0);
            DrawRoundButton(canvas, b_x, b_y, "B", (pressed & GBButton.B) != 0);
            DrawPill(canvas, select_rect, "SELECT", (pressed & GBButton.Select) != 0);
            DrawPill(canvas, start_rect, "START", (pressed & GBButton.Start) != 0);
            DrawPill(canvas, menu_rect, "ROM", menu_touched);
        }

        private void DrawDPad(Canvas canvas)
        {
            float arm = dpad_r * 0.36f;
            Color normal = Color.Rgb(0x33, 0x33, 0x33), down = Color.Rgb(0x11, 0x11, 0x11);

            fill.Color = normal;
            canvas.DrawRoundRect(new RectF(dpad_x - dpad_r, dpad_y - arm, dpad_x + dpad_r, dpad_y + arm), arm * 0.3f, arm * 0.3f, fill);
            canvas.DrawRoundRect(new RectF(dpad_x - arm, dpad_y - dpad_r, dpad_x + arm, dpad_y + dpad_r), arm * 0.3f, arm * 0.3f, fill);

            fill.Color = down;
            if ((pressed & GBButton.Left) != 0) canvas.DrawRect(dpad_x - dpad_r, dpad_y - arm, dpad_x - arm, dpad_y + arm, fill);
            if ((pressed & GBButton.Right) != 0) canvas.DrawRect(dpad_x + arm, dpad_y - arm, dpad_x + dpad_r, dpad_y + arm, fill);
            if ((pressed & GBButton.Up) != 0) canvas.DrawRect(dpad_x - arm, dpad_y - dpad_r, dpad_x + arm, dpad_y - arm, fill);
            if ((pressed & GBButton.Down) != 0) canvas.DrawRect(dpad_x - arm, dpad_y + arm, dpad_x + arm, dpad_y + dpad_r, fill);
        }

        private void DrawRoundButton(Canvas canvas, float x, float y, string label, bool down)
        {
            fill.Color = down ? Color.Rgb(0x6A, 0x12, 0x3A) : Color.Rgb(0x9A, 0x22, 0x57);
            canvas.DrawCircle(x, y, button_r, fill);
            text.Color = Color.Rgb(0x2A, 0x2A, 0x5A);
            text.TextSize = button_r * 0.7f;
            canvas.DrawText(label, x, y + button_r + text.TextSize * 1.1f, text);
        }

        private void DrawPill(Canvas canvas, RectF rect, string label, bool down)
        {
            fill.Color = down ? Color.Rgb(0x55, 0x55, 0x55) : Color.Rgb(0x88, 0x88, 0x88);
            float r = rect.Height() / 2;
            canvas.DrawRoundRect(rect, r, r, fill);
            text.Color = Color.Rgb(0x2A, 0x2A, 0x5A);
            text.TextSize = rect.Height() * 0.42f;
            canvas.DrawText(label, rect.CenterX(), rect.Bottom + text.TextSize * 1.2f, text);
        }

        #endregion

        #region Touch input

        public override bool OnTouchEvent(MotionEvent e)
        {
            int index = e.ActionIndex;
            switch (e.ActionMasked)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:
                    {
                        float x = e.GetX(index), y = e.GetY(index);
                        if (Hit(menu_rect, x, y) || (Message != null && screen_rect.Contains((int)x, (int)y)))
                        {
                            menu_touched = true;
                            touches[e.GetPointerId(index)] = GBButton.None;
                            Invalidate();
                            break;
                        }
                        touches[e.GetPointerId(index)] = ButtonsAt(x, y);
                        break;
                    }

                case MotionEventActions.Move:
                    for (int i = 0; i < e.PointerCount; i++)
                    {
                        int id = e.GetPointerId(i);
                        if (touches.ContainsKey(id) && !menu_touched) touches[id] = ButtonsAt(e.GetX(i), e.GetY(i));
                    }
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.PointerUp:
                    touches.Remove(e.GetPointerId(index));
                    if (menu_touched && touches.Count == 0)
                    {
                        menu_touched = false;
                        if (MenuRequested != null) MenuRequested();
                    }
                    break;

                case MotionEventActions.Cancel:
                    touches.Clear();
                    menu_touched = false;
                    break;
            }

            GBButton now = GBButton.None;
            foreach (GBButton b in touches.Values) now |= b;
            SetPressed(now);
            return true;
        }

        private static bool Hit(RectF r, float x, float y)
        {
            float pad = r.Height() * 0.5f;
            return x >= r.Left - pad && x <= r.Right + pad && y >= r.Top - pad && y <= r.Bottom + pad;
        }

        private GBButton ButtonsAt(float x, float y)
        {
            //D-pad: 8 directions from the angle, with a small dead zone in the middle
            float dx = x - dpad_x, dy = y - dpad_y;
            float dist = (float)Math.Sqrt(dx * dx + dy * dy);
            if (dist <= dpad_r * 1.35f)
            {
                if (dist < dpad_r * 0.15f) return GBButton.None;
                float nx = dx / dist, ny = dy / dist;
                GBButton dir = GBButton.None;
                if (nx > 0.38f) dir |= GBButton.Right;
                if (nx < -0.38f) dir |= GBButton.Left;
                if (ny > 0.38f) dir |= GBButton.Down;
                if (ny < -0.38f) dir |= GBButton.Up;
                return dir;
            }

            //A and B: the area between them presses both
            float da = Distance(x, y, a_x, a_y), db = Distance(x, y, b_x, b_y);
            float reach = button_r * 1.35f;
            if (da <= reach && db <= reach) return GBButton.A | GBButton.B;
            if (da <= reach) return GBButton.A;
            if (db <= reach) return GBButton.B;

            if (Hit(select_rect, x, y)) return GBButton.Select;
            if (Hit(start_rect, x, y)) return GBButton.Start;
            return GBButton.None;
        }

        private static float Distance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        //Also used for hardware keys and gamepads
        public void SetPressed(GBButton now)
        {
            if (now == pressed) return;
            if (((now & ~pressed) != 0)) PerformHapticFeedback(FeedbackConstants.VirtualKey);
            pressed = now;
            if (ButtonsChanged != null) ButtonsChanged(now);
            Invalidate();
        }

        public GBButton PressedButtons
        {
            get { return pressed; }
        }

        public void ReleaseAll()
        {
            touches.Clear();
            menu_touched = false;
            SetPressed(GBButton.None);
        }

        #endregion
    }
}
