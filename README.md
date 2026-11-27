# ZeXal
ZeXal is an emulator lib. for the Sharp LR35902, a cpu based on the Z80 and commonly found on Nintendo Game Boy Handhelds

## Projects

- **ZeXal.CPUCore**: the LR35902 CPU (opcodes, registers, interrupt hooks).
- **ZeXal.DMG**: the rest of the DMG-001 hardware: PPU (graphics), memory map, cartridge
  (ROM only, MBC1, MBC3 without RTC, MBC5), interrupts, timer, serial port, joypad.
  `GameBoy` puts everything together.
- **ZeXal.Test**: console front end.

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
