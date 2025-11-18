; main.asm
                        OPT RESET --syntax=abfw \
                            --zxnext=cspect             ; Tighten up syntax and warnings
                        DEVICE ZXSPECTRUMNEXT           ; Use ZX Spectrum Next memory model
                        MMU 0 3, 6                      ; slot 0=8K page  6, s1=p7, s2=p8, s3=p9
                        MMU 4 5, 12                     ; slot 4=8K page 12, s5=p13
                        MMU 6 7, 0                      ; slot 6=8K page  0, s7=p1

                        INCLUDE "constants.asm"         ; Constants
                        INCLUDE "macros.asm"            ; Macros

                        ORG $c000                       ; Setup code lives in upper 16K                      
Start:
                        di                              ; Disable interrupts while setting up
                        nextreg 7, %10                  ; Set 14MHz (avoids extra wait states present at 28MHz)
                        NRREAD 105                      ; Read layer 2 state
                        ld (L2Enable), a                ;   and preserve it.
                        and %0'111'1111                 ; Disable layer 2
                        nextreg 105, a                  ;   and apply it.

                        SETUPL2 18, %000'000'11         ; Fill 10x 8K layer 2 banks
                        SETUPL2 19, %000'111'00         ; with alternate blue/green stripes
                        SETUPL2 20, %000'000'11
                        SETUPL2 21, %000'111'00
                        SETUPL2 22, %000'000'11
                        SETUPL2 23, %000'111'00
                        SETUPL2 24, %000'000'11
                        SETUPL2 25, %000'111'00
                        SETUPL2 26, %000'000'11
                        SETUPL2 27, %000'111'00
                       
                        SETUPL2 28, %111'111'00         ; Fill 10x 8K layer 2 banks
                        SETUPL2 29, %000'000'00         ; with alternate yellow/black stripes
                        SETUPL2 30, %111'111'00
                        SETUPL2 31, %000'000'00
                        SETUPL2 32, %111'111'00
                        SETUPL2 33, %000'000'00
                        SETUPL2 34, %111'111'00
                        SETUPL2 35, %000'000'00
                        SETUPL2 36, %111'111'00
                        SETUPL2 37, %000'000'00

                        nextreg 18, 9                   ; Set base layer 2 bank to 16K bank 9 (green/blue)     
                        nextreg 24, 0                   ; Set max clip window for 320x256 layer 2
                        nextreg 24, 255
                        nextreg 24, 0
                        nextreg 24, 255

                        nextreg 20, 0                   ; Set global transparency to black (RGB332)
                        nextreg 74, 0                   ; Set transparency fallback to black (RGB332)
                        nextreg 21, %000'010'00         ; Put sprites on top, then ULA, then layer 2 (SUL)
                        nextreg $52, 10                 ; Page in ULA screen
                        FILLLDIR $4000, $1800, 0        ; Clear ULA pixels
                        FILLLDIR $5800,$300,%00'000'001 ; Clear ULA pixels to dim yellow ink, black paper
                        nextreg 67, %0'100'011'0        ; Select ULA alt palette for writing and display
                        nextreg 64, 16                  ; Set palette write index to ULA dim black paper
                        nextreg 65, %000'000'00         ; Redefine ULA dim black paper as black (RGB332)

                        nextreg $50, 255
                        nextreg $51, 255
                        nextreg $53, 11

                        ld ixl, Draw.Rows               ; Process every row in the draw table
                        ld hl, Draw.Table
.RowLoop:               ld e, (hl)
                        inc hl
                        ld d, (hl)                      ; de = draw address
                        inc hl
                        ld b, (hl)                      ; b  = draw count
                        inc hl
                        ld a, (hl)                      ; a' = address increment
                        ex af, af'
                        inc hl
                        ld a, (hl)                      ; a  = draw value)
.DrawLoop:              ld (de), a                      ; Write value to draw address
                        ex af, af'
                        add de, a                       ; Increase draw address by Inc
                        ex af, af'
                        dec b
                        jr nz, .DrawLoop                ; Write more values to draw address until Count is reached
                        inc hl
                        dec ixl
                        jr nz, .RowLoop                 ; Process the next row in the draw table

                        ld a, 2
                        call ROM3_CHAN_OPEN
                        ld a, %01'010'111
                        ld (ROM3_ATTR_TL), a
                        NRREAD 17                       ; Read video mode (VGA0..6, HDMI)
                        and %111
                        rlca
                        rlca
                        ld hl, Timing.Table             ; Copy video mode text to message
                        add hl, a
                        ld de, Message.Timing
                        ld bc, 4
                        ldir
                        NRREAD 5                        ; Read FPS(50/60)
                        and %100
                        rrca
                        ld hl, Fps.Table                ; Copy FPS text to message
                        add hl, a
                        ld de, Message.Fps
                        ld bc, 2
                        ldir
                        NRREAD 1                        ; Read core major/minor version (xxx.yyy)
                        push af
                        swapnib
                        and %1111
                        ld de, Core.Table               ; Write core major version to buffer
                        call DispA
                        pop af
                        and %1111                        
                        call DispA                      ; Write core minor version to buffer
                        NRREAD 14                       ; Read core sub version (.zzz)
                        call DispA                      ; Write core minor version to buffer 
                        ld a, '.'                       ; Format core text with dots
                        ld (Core.Table+3), a
                        ld (Core.Table+6), a
                        ld de, Message.Core             ; Copy core text to message without leading zeroes
                        ld hl, Core.Table
                        ld a, (hl)
                        cp '0'
                        jr nz, .CopyCore
                        inc hl
                        ld a, (hl)
                        cp '0'
                        jr nz, .CopyCore
                        inc hl
.CopyCore:              ld a, (hl)
.NextChar:              ld (de), a
                        inc de
                        inc hl
                        ld a, (hl)
                        cp 0
                        jr nz, .NextChar
                        NRREAD 15                       ; Read core issue
                        and %11                         ; We don't have print routines for anything newer than issue 5 so truncate
                        ld hl, Issue.Table
                        add hl, a
                        ld a, (hl)                      ; Write core issue text to message
                        ld (Message.Issue), a
                        NRREAD 7                        ; Read CPU speed (0..3)
                        and %11                         ; We don't have print routines for anything faster than 28 so truncate
                        rlca
                        rlca
                        rlca
                        ld hl, CPU.Table
                        add hl, a                       ; Write CPU speed text to message
                        ld de, Message.CPU
                        ld bc, 8
                        ldir
                        NRREAD 3                        ; Read machine display timing
                        swapnib
                        and %111                        ; Isolate display timing
                        dec a                           ; and subtract 1 to put in range 0..3
                        ld d, a
                        ld e, 10
                        mul d, e
                        ld a, e
                        ld hl, Machine.Table            ; Copy display timing text to message
                        add hl, a
                        ld de, Message.Machine
                        ld bc, 10
                        ldir                                    
                        PRINT Message.Table, Message.Len; Print message in center of screen
                        
L2Enable+*:             ld a, SMC
                        or %1'000'0000
                        nextreg 112, %00'01'0000        ; Set layer 2 to 320x256 mode                    
                        nextreg 105, a                  ; Enable layer 2
                        nextreg 34, %00000'11'0         ; Disable ULA interrupt and enable line interrupt
                        nextreg 35, 0                   ; Set line interrupt to interrupt at line 0
                        nextreg 100, 32                 ; Set video offset so that line 0 is the first 320x256 line

                        nextreg $50, 6                  ; Set lower 48K to contain the line interrupt handler
                        nextreg $51, 7                  ; with the screen-changing code.
                        nextreg $52, 8
                        nextreg $53, 9
                        nextreg $54, 12
                        nextreg $55, 13

                        jp LoadImage

                        ld a, $fd                       ; Setup mode 2 interrupts for vector table at $fd00 to $fe01
                        ld i, a                         ; This points to $0000 where the screen-updating code lives
                        im 2
                        ei                              ; Finally enable mode 2 line interrupts                     
MainLoop:                   
                        //adc hl, bc                      ; The main loop only contains timing padding in a tight loop
                        jp MainLoop                     ; All the work is done in the line interrupt
FillBank:                  
                        nextreg $50, a                  ; Fill an 8K bank whose bank number is in b
                        ld hl, $0000                    ; with the byte value in a.
                        ld (hl), b
                        ld de, $0001
                        ld bc, $1fff
                        ldir
                        ret
DispA:                  ld c, -100                      ; From http://wikiti.brandonw.net/index.php?title=Z80_Routines:Other:DispA
                        call .Na1
                        ld	c, -10
                        call .Na1
	                    ld	c, -1
.Na1:                   ld	b, '0'-1
.Na2:                   inc	b
	                    add	a, c
                        jr	c, .Na2
                        sub	c
                        push af
                        ld	a, b
	                    ld (de), a
                        inc de
	                    pop af
	                    ret

                        INCLUDE "tables.asm"
ImagePal:
                        INCBIN "../../data/park_rgb333.pal"
LoadImage:
                        nextreg 67, %0'101'011'0        ; Select layer 2 alt palette for writing and display
                        nextreg 64, 0                   ; Reset palette write index
                        ld b, 0                         ; Loop 256 times, once for each palette entry
                        ld hl, ImagePal
.Loop:                  ld a, (hl)
                        nextreg 68, a                   ; Write LSB of RGB333 palette colour
                        inc hl
                        ld a, (hl)
                        nextreg 68, a                   ; Write MSB of RGB333 palette colour
                        inc hl
                        djnz .Loop                      ; Continue for other palette entries                      
                        nextreg 18, 24
                        jr $ 

                        DISPLAY "Last address before IM2 vector=", $
                        ASSERT $ < $fd00,                 Code is >= $fd00
Im2Vector:
                        ORG $fd00                       ; IM2 vector table points to ISR at $0000,
                        REPT 257                        ; whatever value is placed on the bus for the LSB.
                            DB $00
                        ENDR

                        DISPLAY "Last Address=",$

                        INCLUDE "screen.asm"
                        INCLUDE "images.asm"

                        SAVENEX OPEN "../../bin/L2TimingTest.nex", Start, $0000
                        SAVENEX CORE 3, 01, 05          ; Next core 3.01.05 required as minimum
                        //SAVENEX SCREEN BMP "../../img/loading-screen3.bmp"
                        SAVENEX BANK 0, 3, 4, 6, 24, 25, 26, 27, 28
                        SAVENEX CLOSE