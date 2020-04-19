{
    "interactionModel": {
        "languageModel": {
            "invocationName": "pádel info",
            "intents": [
                {
                    "name": "AMAZON.CancelIntent",
                    "samples": []
                },
                {
                    "name": "AMAZON.HelpIntent",
                    "samples": []
                },
                {
                    "name": "AMAZON.StopIntent",
                    "samples": []
                },
                {
                    "name": "AMAZON.NavigateHomeIntent",
                    "samples": []
                },
                {
                    "name": "GetPlayersAtPositionIntent",
                    "slots": [
                        {
                            "name": "position",
                            "type": "AMAZON.NUMBER"
                        },
                        {
                            "name": "rankingCategory",
                            "type": "RankingCategorySlotType"
                        },
                        {
                            "name": "rankingType",
                            "type": "RankingTypeSlotType"
                        },
                        {
                            "name": "spanishSingularGender",
                            "type": "SpanishSingularGenderSlotType"
                        }
                    ],
                    "samples": [
                        "quién es {spanishSingularGender} mejor jugador del {rankingType}",
                        "quién es {spanishSingularGender} campeón",
                        "quién es {spanishSingularGender} mejor jugador",
                        "quién es {spanishSingularGender} top {position} {rankingCategory} del {rankingType}",
                        "quién es {spanishSingularGender} top {position} {rankingCategory}",
                        "quién es {spanishSingularGender} top {position}",
                        "quién es {spanishSingularGender} número {position} {rankingCategory}",
                        "quién es {spanishSingularGender} número {position}",
                        "quién es {spanishSingularGender} campeón del {rankingType}",
                        "quién es {spanishSingularGender} campeón {rankingCategory}",
                        "quién es {spanishSingularGender} campeón {rankingCategory} del {rankingType}",
                        "quién es {spanishSingularGender} mejor {rankingCategory} del {rankingType}",
                        "quién es {spanishSingularGender} mejor {rankingCategory}",
                        "quién es {spanishSingularGender} {rankingCategory} número {position} del {rankingType} ",
                        "quién es {spanishSingularGender} número {position} de {rankingCategory} del {rankingType}"
                    ]
                },
                {
                    "name": "GetPlayersAtPositionsIntent",
                    "slots": [
                        {
                            "name": "position",
                            "type": "AMAZON.NUMBER"
                        },
                        {
                            "name": "rankingCategory",
                            "type": "RankingCategorySlotType"
                        },
                        {
                            "name": "rankingType",
                            "type": "RankingTypeSlotType"
                        },
                        {
                            "name": "spanishPluralGender",
                            "type": "SpanishPluralGenderSlotType"
                        }
                    ],
                    "samples": [
                        "quienes son {spanishPluralGender} {position}  mejores {rankingCategory}",
                        "quién son {spanishPluralGender} {position}  mejores {rankingCategory}",
                        "quienes son {spanishPluralGender} {position} mejores jugadores del {rankingType}",
                        "quién son {spanishPluralGender} {position} mejores jugadores del {rankingType}",
                        "quienes son {spanishPluralGender} {position} mejores jugadores",
                        "quién son {spanishPluralGender} {position} mejores jugadores",
                        "quienes son {spanishPluralGender} {position} mejores",
                        "quién son {spanishPluralGender} {position} mejores",
                        "quienes son {spanishPluralGender} {position} mejores  {rankingCategory} del {rankingType}",
                        "quién son {spanishPluralGender} {position} mejores  {rankingCategory} del {rankingType}",
                        "quienes son {spanishPluralGender} mejores {position} {rankingCategory}",
                        "quién son {spanishPluralGender} mejores {position} {rankingCategory}",
                        "quienes son {spanishPluralGender} mejores {position} jugadores del {rankingType}",
                        "quién son {spanishPluralGender} mejores {position} jugadores del {rankingType}",
                        "quienes son {spanishPluralGender} top {position} jugadores del {rankingType}",
                        "quién son {spanishPluralGender} top {position} jugadores del {rankingType}",
                        "quienes son {spanishPluralGender} top {position} jugadores",
                        "quién son {spanishPluralGender} top {position} jugadores",
                        "quienes son {spanishPluralGender} mejores {position} jugadores",
                        "quién son {spanishPluralGender} mejores {position} jugadores",
                        "quienes son {spanishPluralGender} mejores {position}",
                        "quién son {spanishPluralGender} mejores {position}",
                        "quienes son {spanishPluralGender} top {position} {rankingCategory}",
                        "quién son {spanishPluralGender} top {position} {rankingCategory}",
                        "quienes son {spanishPluralGender} top {position}",
                        "quién son {spanishPluralGender} top {position}",
                        "quienes son {spanishPluralGender} top {position} {rankingCategory} del {rankingType}",
                        "quienes son {spanishPluralGender} mejores {position} {rankingCategory} del {rankingType}",
                        "quién son {spanishPluralGender} top {position} {rankingCategory} del {rankingType}",
                        "quién son {spanishPluralGender} mejores {position} {rankingCategory} del {rankingType}"
                    ]
                },
                {
                    "name": "GetTournamentsIntent",
                    "slots": [
                        {
                            "name": "rankingType",
                            "type": "RankingTypeSlotType"
                        }
                    ],
                    "samples": [
                        "cuándo son los próximo torneos",
                        "cuándo es el próximo torneo",
                        "cuándo es el próximo torneo del {rankingType}"
                    ]
                }
            ],
            "types": [
                {
                    "name": "RankingTypeSlotType",
                    "values": [
                        {
                            "name": {
                                "value": "l. t. a. padel",
                                "synonyms": [
                                    "british padel",
                                    "padel u. k.",
                                    "british padel tour",
                                    "l t a padel tour",
                                    "l t a padel",
                                    "l. t. a.",
                                    "l. t. a. padel tour"
                                ]
                            }
                        },
                        {
                            "name": {
                                "value": "world padel tour",
                                "synonyms": [
                                    "w p t",
                                    "w. p. t."
                                ]
                            }
                        }
                    ]
                },
                {
                    "name": "RankingCategorySlotType",
                    "values": [
                        {
                            "name": {
                                "value": "juvenil",
                                "synonyms": [
                                    "jovenes",
                                    "joven",
                                    "niñas",
                                    "niños",
                                    "niña",
                                    "niño",
                                    "juveniles"
                                ]
                            }
                        },
                        {
                            "name": {
                                "value": "femenino senior",
                                "synonyms": [
                                    "mujeres senior",
                                    "mujer senior",
                                    "chicas senior",
                                    "chica senior"
                                ]
                            }
                        },
                        {
                            "name": {
                                "value": "masculino senior",
                                "synonyms": [
                                    "hombres senior",
                                    "hombre senior",
                                    "chicos senior",
                                    "chico senior"
                                ]
                            }
                        },
                        {
                            "name": {
                                "value": "femenino",
                                "synonyms": [
                                    "mujeres",
                                    "mujer",
                                    "chicas",
                                    "chica",
                                    "femeninos"
                                ]
                            }
                        },
                        {
                            "name": {
                                "value": "masculino",
                                "synonyms": [
                                    "hombres",
                                    "hombre",
                                    "chicos",
                                    "chico",
                                    "masculinos"
                                ]
                            }
                        }
                    ]
                },
                {
                    "name": "SpanishPluralGenderSlotType",
                    "values": [
                        {
                            "name": {
                                "value": "las"
                            }
                        },
                        {
                            "name": {
                                "value": "los"
                            }
                        }
                    ]
                },
                {
                    "name": "SpanishSingularGenderSlotType",
                    "values": [
                        {
                            "name": {
                                "value": "la"
                            }
                        },
                        {
                            "name": {
                                "value": "el"
                            }
                        }
                    ]
                }
            ]
        }
    }
}