{
    "interactionModel": {
        "languageModel": {
            "invocationName": "padel info",
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
                            "name": "numberTop",
                            "type": "NumberTopSlotType"
                        }
                    ],
                    "samples": [
                        "Who is the {numberTop} {position} {rankingCategory} player on {rankingType}",
                        "Who is the best player on {rankingType}",
                        "Who is the top player on {rankingType}",
                        "Who is the {numberTop} {position} on {rankingType}",
                        "Who is the {numberTop} {position} on the {rankingType}",
                        "Who is the top player on the {rankingType}",
                        "Who is the best player on the {rankingType}",
                        "Who is the {rankingType} champion",
                        "Who is the best player",
                        "Who is the {numberTop} {position} {rankingCategory} player on the {rankingType}",
                        "Who is the {numberTop} {position} {rankingCategory} player",
                        "Who is the {numberTop} {position} player",
                        "Who is the top player",
                        "Who is the {numberTop} {position} {rankingCategory}",
                        "Who is the {numberTop} {position}",
                        "Who is the best on the {rankingType}",
                        "Who is the best {rankingCategory}",
                        "Who is the {rankingCategory} champion on the {rankingType}",
                        "Who is the best {rankingCategory} player on the {rankingType}",
                        "Who is the {rankingCategory} champion",
                        "Who is the best {rankingCategory} player",
                        "Who is the {numberTop} {position} {rankingCategory} on the {rankingType}"
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
                            "name": "bestTop",
                            "type": "BestTopSlotType"
                        }
                    ],
                    "samples": [
                        "who are the {bestTop} {position} players on the {rankingType}",
                        "who are the {bestTop} {position} players",
                        "who are the {bestTop} {position} {rankingCategory}",
                        "who are the {bestTop} {position} ",
                        "who are the {bestTop} {position} {rankingCategory} on the {rankingType}"
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
                        "When are the next tournaments",
                        "When is the next tournament",
                        "When is the next {rankingType} tournament"
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
                                "value": "junior",
                                "synonyms": [
                                    "juniors"
                                ]
                            }
                        },
                        {
                            "name": {
                                "value": "senior female",
                                "synonyms": [
                                    "senior ladies",
                                    "senior lady",
                                    "senior females"
                                ]
                            }
                        },
                        {
                            "name": {
                                "value": "senior male",
                                "synonyms": [
                                    "senior men",
                                    "senior males",
                                    "senior men's"
                                ]
                            }
                        },
                        {
                            "name": {
                                "value": "female",
                                "synonyms": [
                                    "girls",
                                    "girl",
                                    "ladies",
                                    "lady",
                                    "females"
                                ]
                            }
                        },
                        {
                            "name": {
                                "value": "male",
                                "synonyms": [
                                    "boys",
                                    "boy",
                                    "men's",
                                    "men",
                                    "males"
                                ]
                            }
                        }
                    ]
                },
                {
                    "name": "BestTopSlotType",
                    "values": [
                        {
                            "name": {
                                "value": "top"
                            }
                        },
                        {
                            "name": {
                                "value": "best"
                            }
                        }
                    ]
                },
                {
                    "name": "NumberTopSlotType",
                    "values": [
                        {
                            "name": {
                                "value": "top"
                            }
                        },
                        {
                            "name": {
                                "value": "number"
                            }
                        }
                    ]
                }
            ]
        }
    }
}