; screen.asm
                        ORG $0000
Screen:
                        BORDER 3                        ; Constant timing so we can change colours
                        REPT 5838                       ; At VGA50 14MHz, this covers 320x256 layer 2. 5838 x 2 = 11676
                            nextreg 18, 9
                            nextreg 18, 14
                        ENDR
                        nextreg 18, 9                   ; This occurs after the end of 320x256 layer 2
                        BORDER 1                        ; Constant timing so we can change colours
                        REPT 7                          ; Any necessary timing padding
                            cp 0
                        ENDR

                        ei
                        reti

                        DISPLAY "Last Screen=", $