; screen.asm
                        ORG $0000
Screen:
                        out (ULA_PORT), A               ; More precise to start ISR with OUT
                        //BORDER 3                      ; Constant timing so we can change colours
                        REPT 5838                       ; At VGA50 14MHz +3 timing this covers 320x256 layer 2
                            nextreg 18, 9               ; 5,838 x 2 = 11,676 
                            nextreg 18, 14              ; 11,676 x 20T = 233,520T
                        ENDR
                        nextreg 18, 9                   ; This occurs after the end of 320x256 layer 2
                        BORDER 1                        ; Constant timing so we can change colours
                        REPT 7                          ; Any necessary timing padding
                            cp 0
                        ENDR
                        ld a, 3                         ; Set A for border change next frame     
                        ei
                        reti

                        DISPLAY "Last Screen=", $