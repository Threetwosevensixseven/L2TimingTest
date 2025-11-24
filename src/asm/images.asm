; images.asm
                        MMU 0 n, 48, $0000              ; "n" option to auto-wrap with next page on slot 0 boundary
                        INCBIN "../../data/park_rgb333.bin"  ; 320x256 80kiB pixel data read into pages 48..57
ImageEnd:
                        ASSERT $$ImageEnd=58 & $=$0000,   Image is not 80kiB

                        IFDEF INCPATTERN
                            MMU 0 n, 18, $0000
                            
                            INCBIN "../../data/pattern_18.bin"
                            INCBIN "../../data/pattern_19.bin"
                            INCBIN "../../data/pattern_20.bin"
                            INCBIN "../../data/pattern_21.bin"
                            INCBIN "../../data/pattern_22.bin"
                            INCBIN "../../data/pattern_23.bin"
                            INCBIN "../../data/pattern_24.bin"
                            INCBIN "../../data/pattern_25.bin"
                            INCBIN "../../data/pattern_26.bin"
                            INCBIN "../../data/pattern_27.bin"

                            INCBIN "../../data/pattern_28.bin"
                            INCBIN "../../data/pattern_29.bin"
                            INCBIN "../../data/pattern_30.bin"
                            INCBIN "../../data/pattern_31.bin"
                            INCBIN "../../data/pattern_32.bin"
                            INCBIN "../../data/pattern_33.bin"
                            INCBIN "../../data/pattern_34.bin"
                            INCBIN "../../data/pattern_35.bin"
                            INCBIN "../../data/pattern_36.bin"
                            INCBIN "../../data/pattern_37.bin"
                        ENDIF

                        MMU 0, 6                        ; Restore previous MMU and turn off slot auto-wrap