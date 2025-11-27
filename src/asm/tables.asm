; tables.asm

Message.Table:          DB AT, TEXTY+0, TEXTX
Message.Timing:         DB "          "
                        DB AT, TEXTY+1, TEXTX
Message.Fps:            DB "   Hz     "
                        DB AT, TEXTY+2, TEXTX
Message.Core:           DB "          "
                        DB AT, TEXTY+3, TEXTX, "Issue "
Message.Issue:          DB "    "
                        DB AT, TEXTY+4, TEXTX
Message.CPU:            DB "          "
                        DB AT, TEXTY+5, TEXTX
Message.Machine:        DB "          "
Message.Len             EQU $-Message.Table

Core.Table:             
                        DB "         ", 0

Timing.Table:           
                        DB "VGA0"
                        DB "VGA1"
                        DB "VGA2"
                        DB "VGA3"
                        DB "VGA4"
                        DB "VGA5"
                        DB "VGA6"
                        DB "HDMI"

Fps.Table:              
                        DB "50"
                        DB "60"

Issue.Table:            
                        DB "2"
                        DB "3"
                        DB "4"
                        DB "5"

CPU.Table:              
                        DB "3.5 MHz "
                        DB "7 MHz   "
                        DB "14 MHz  "
                        DB "28 MHz  "

Machine.Table:          
                        DB "48K Timing"
                        DB "128 Timing"
                        DB "+3 Timing "
                        DB "Pentagon  "

Draw.Table:
                        ; Address    Count  Inc  Val  Row  Notes
                        ; -------   ------  ---  ---  ---  ---------------------
                        DW  $4F0B : DB $0A, $01, $FF ;  0  Top side
                        DW  $48EB : DB $0A, $01, $FF ;  1  Bottom side 1
                        DW  $49EB : DB $0A, $01, $FF ;  2  Bottom side 2
                        DW  $482A : DB $06, $20, $01 ;  3  Left side
                        DW  $492A : DB $06, $20, $01 ;  4  
                        DW  $4A2A : DB $06, $20, $01 ;  5  
                        DW  $4B2A : DB $06, $20, $01 ;  6  
                        DW  $4C2A : DB $06, $20, $01 ;  7  
                        DW  $4D2A : DB $06, $20, $01 ;  8  
                        DW  $4E2A : DB $06, $20, $01 ;  9  
                        DW  $4F2A : DB $06, $20, $01 ; 10  
                        DW  $4835 : DB $06, $20, $C0 ; 11  Right side
                        DW  $4935 : DB $06, $20, $C0 ; 12  
                        DW  $4A35 : DB $06, $20, $C0 ; 13  
                        DW  $4B35 : DB $06, $20, $C0 ; 14  
                        DW  $4C35 : DB $06, $20, $C0 ; 15  
                        DW  $4D35 : DB $06, $20, $C0 ; 16  
                        DW  $4E35 : DB $06, $20, $C0 ; 17  
                        DW  $4F35 : DB $06, $20, $C0 ; 18  
                        DW  $48EA : DB $01, $00, $01 ; 19  Top left corner
                        DW  $4F0A : DB $01, $00, $01 ; 20  Bottom left corner
                        DW  $48F5 : DB $01, $00, $C0 ; 21  Bottom right corner 1
                        DW  $49F5 : DB $01, $00, $C0 ; 22  Bottom right corner 2
                        DW  $4F15 : DB $01, $00, $80 ; 23  Top right corner
Draw.Size               EQU 5
Draw.Len                EQU $-Draw.Table
Draw.Rows               EQU Draw.Len/Draw.Size

Copper.Program:
                        COPPER_WAIT 0, 1                ; Wait for line 1,
                        COPPER_WAIT 0, 0                ;   then line 0, ensuring at least one frame elapses.
Copper.ShowL2:          COPPER_MOVE 105, SMC            ; Only then, enable Layer 2 to avoid flash of scrambled date.
                        COPPER_WAITFRAMES 149           ; Wait for another 149 frames,
Copper.NoUla:           COPPER_MOVE 104, SMC            ;   then disable ULA layer, hiding info panel.
                        COPPER_HALT                     ; Finally, halt copper program without repeating.
Copper.InstructionCount EQU ($-Copper.Program)/2        ; Instruction size: 2 bytes, capacity: 1024 instructions.
