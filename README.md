# ZeXal
ZeXal is an emulator lib. for the Sharp LR35902, a cpu based on the Z80 and commonly found on Nintendo Game Boy Handhelds

## Projects

- **ZeXal.CPUCore**: the LR35902 CPU (opcodes, registers, interrupt hooks).
- **ZeXal.DMG**: the rest of the DMG-001 hardware: PPU (graphics), memory map, cartridge
  (ROM only, MBC1, MBC3 without RTC, MBC5), interrupts, timer, serial port, joypad.
  `GameBoy` puts everything together.
- **ZeXal.Test**: console front end (test ROM runner).
- **ZeXal.Viewer**: Windows Forms front end that shows the display in real time.
- **ZeXal.Android**: Android app (.NET 9), with on-screen controls. Not part of `ZeXal.sln`.

## Viewer

```
ZeXal.Viewer.exe [rom.gb] [--scale 1-6]
```

Open a ROM with Ctrl+O or by dragging it on the window.

| Key | Game Boy |
| --- | --- |
| Arrows | D-pad |
| X / Z | A / B |
| Enter / Backspace | Start / Select |
| Tab (hold) | Fast forward |

F5 resets, Ctrl+P pauses, Ctrl+1..6 change the window scale, Ctrl+S saves a PNG screenshot.
The View menu switches between the DMG green and the grayscale palette.

## Running a test ROM

```
ZeXal.Test.exe --test-rom cpu_instrs.gb [--frames 6000] [--screenshot out.bmp] [--trace]
```

The ROM runs headless. The result is detected from the serial output or cartridge RAM
(Blargg test ROMs) or from the `LD B,B` breakpoint (Mooneye test ROMs). For graphics tests
such as dmg-acid2, use `--screenshot` to save the last frame.
Exit code: 0 passed, 1 failed, 2 no result within `--frames`, 3 error.

Without arguments, `ZeXal.Test` steps through `Binaries/test.vm` as before.

## PPU limitations

The PPU renders one scanline at a time at the end of mode 3, which has a fixed length.
Effects that depend on changing registers in the middle of a scanline are not reproduced,
and CPU access to VRAM/OAM is never blocked. OAM DMA is instantaneous.

## Android

Requires the .NET 9 SDK with the `android` workload, a JDK 17 and the Android SDK (platform 35):

```
dotnet workload install android
dotnet publish ZeXal.Android/ZeXal.Android.csproj -c Release -p:AndroidSdkDirectory=<android-sdk> -p:JavaSdkDirectory=<jdk>
```

The signed APK is `ZeXal.Android/bin/Release/net9.0-android/publish/io.github.delfioh.zexal-Signed.apk`
(signed with the debug key; minimum Android 5.0). Tap "ROM" to open a `.gb` file; the last ROM reopens
at the next start and battery saves are kept in the app storage. Gamepads and keyboards work too
(D-pad, A/B or X/Z, Start/Enter, Select/Del).
