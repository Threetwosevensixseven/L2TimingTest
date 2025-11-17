; constants.asm

; Misc
SMC                     EQU 0
TEXTX                   EQU (32-10)/2
TEXTY                   EQU (24-6)/2

; Ports
ULA_PORT                EQU $e                          ; out (254), a

; ROM routines
ROM3_CHAN_OPEN          EQU $1601                       ; 5633
ROM3_PRINT              EQU $203c                       ; 8252

; ROM sysvars
ROM3_ATTR_TL           EQU $5c8f

; Printing
CR                      EQU 13
INK                     EQU 16
PAPER                   EQU 17
FLASH                   EQU 18
INVERSE                 EQU 20
OVER                    EQU 21
AT                      EQU 22