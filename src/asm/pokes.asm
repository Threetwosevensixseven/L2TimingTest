; pokes.asm

DoPokes:                
                        nextreg $50, 18

                        ;ld a, %111'000'00
                        ;ld ($0100), a
                        ;ld ($0101), a
                        ;ld ($0104), a
                        ;ld ($0105), a


                        ;FILLLDIR $0000, $100, %000'111'11
                        ;FILLLDIR $0100, $100, %111'000'00
                        ;FILLLDIR $0200, $100, %111'000'11


                        ;CSBREAK
                        ret