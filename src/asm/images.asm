; images.asm
                        MMU 0 n, 48, $0000              ; "n" option to auto-wrap with next page on slot 0 boundary
                        INCBIN "../../data/park.bin"  ; 320x256 80kiB pixel data read into pages 48..57
ImageEnd:
                        ASSERT $$ImageEnd=58 & $=$0000,   Image is not 80kiB
                        MMU 0, 6                        ; Restore previous MMU and turn off slot auto-wrap