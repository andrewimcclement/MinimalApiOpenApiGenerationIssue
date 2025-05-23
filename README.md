# MinimalApiOpenApiGenerationIssue
Nested recursive types fail to generate a valid OpenAPI schema, even when not using arrays.

## Types
Note that `Bar2` is a recursive type.
```csharp
[JsonDerivedType(typeof(Foo1), "foo1")]
[JsonDerivedType(typeof(Foo2), "foo2")]
public abstract record Foo(Bar Bar);
public sealed record Foo1(Bar Bar, string Haha = "boo") : Foo(Bar);
public sealed record Foo2(Bar Bar, int Musketeer = 3) : Foo(Bar);

[JsonDerivedType(typeof(Bar1), "bar1")]
[JsonDerivedType(typeof(Bar2), "bar2")]
public abstract record Bar;
public sealed record Bar1(string Hi = "Hi") : Bar;
public sealed record Bar2(Bar Left, Bar Right) : Bar;
```

## OpenAPI schemas

<details>
<summary>By default - note it gives up after a certain depth</summary>

This can be generated by running the `https` profile.

```json
{
  "openapi": "3.0.1",
  "info": {
    "title": "Issue | v1",
    "version": "1.0.0"
  },
  "servers": [
    {
      "url": "https://localhost:7242/"
    }
  ],
  "paths": {
    "/foo": {
      "get": {
        "tags": [
          "Issue"
        ],
        "operationId": "GetFoo",
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Foo"
                }
              }
            }
          }
        }
      }
    }
  },
  "components": {
    "schemas": {
      "Foo": {
        "required": [
          "$type"
        ],
        "type": "object",
        "anyOf": [
          {
            "$ref": "#/components/schemas/FooFoo1"
          },
          {
            "$ref": "#/components/schemas/FooFoo2"
          }
        ],
        "discriminator": {
          "propertyName": "$type",
          "mapping": {
            "foo1": "#/components/schemas/FooFoo1",
            "foo2": "#/components/schemas/FooFoo2"
          }
        }
      },
      "FooFoo1": {
        "required": [
          "bar"
        ],
        "properties": {
          "$type": {
            "enum": [
              "foo1"
            ],
            "type": "string"
          },
          "haha": {
            "type": "string",
            "default": "boo"
          },
          "bar": {
            "required": [
              "$type"
            ],
            "type": "object",
            "anyOf": [
              {
                "properties": {
                  "$type": {
                    "enum": [
                      "bar1"
                    ],
                    "type": "string"
                  },
                  "hi": {
                    "type": "string",
                    "default": "Hi"
                  }
                }
              },
              {
                "required": [
                  "left",
                  "right"
                ],
                "properties": {
                  "$type": {
                    "enum": [
                      "bar2"
                    ],
                    "type": "string"
                  },
                  "left": {
                    "required": [
                      "$type"
                    ],
                    "type": "object",
                    "anyOf": [
                      {
                        "properties": {
                          "$type": {
                            "enum": [
                              "bar1"
                            ],
                            "type": "string"
                          },
                          "hi": {
                            "type": "string",
                            "default": "Hi"
                          }
                        }
                      },
                      {
                        "required": [
                          "left",
                          "right"
                        ],
                        "properties": {
                          "$type": {
                            "enum": [
                              "bar2"
                            ],
                            "type": "string"
                          },
                          "left": {
                            "discriminator": {
                              "propertyName": "$type",
                              "mapping": {
                                "bar1": "#/components/schemas/BarBar1",
                                "bar2": "#/components/schemas/BarBar2"
                              }
                            }
                          },
                          "right": {
                            "required": [
                              "$type"
                            ],
                            "type": "object",
                            "anyOf": [
                              {
                                "properties": {
                                  "$type": {
                                    "enum": [
                                      "bar1"
                                    ],
                                    "type": "string"
                                  },
                                  "hi": {
                                    "type": "string",
                                    "default": "Hi"
                                  }
                                }
                              },
                              {
                                "required": [
                                  "left",
                                  "right"
                                ],
                                "properties": {
                                  "$type": {
                                    "enum": [
                                      "bar2"
                                    ],
                                    "type": "string"
                                  },
                                  "left": {
                                    "discriminator": {
                                      "propertyName": "$type",
                                      "mapping": {
                                        "bar1": "#/components/schemas/BarBar1",
                                        "bar2": "#/components/schemas/BarBar2"
                                      }
                                    }
                                  },
                                  "right": {
                                    "discriminator": {
                                      "propertyName": "$type",
                                      "mapping": {
                                        "bar1": "#/components/schemas/BarBar1",
                                        "bar2": "#/components/schemas/BarBar2"
                                      }
                                    }
                                  }
                                }
                              }
                            ],
                            "discriminator": {
                              "propertyName": "$type",
                              "mapping": {
                                "bar1": "#/components/schemas/BarBar1",
                                "bar2": "#/components/schemas/BarBar2"
                              }
                            }
                          }
                        }
                      }
                    ],
                    "discriminator": {
                      "propertyName": "$type",
                      "mapping": {
                        "bar1": "#/components/schemas/BarBar1",
                        "bar2": "#/components/schemas/BarBar2"
                      }
                    }
                  },
                  "right": {
                    "discriminator": {
                      "propertyName": "$type",
                      "mapping": {
                        "bar1": "#/components/schemas/BarBar1",
                        "bar2": "#/components/schemas/BarBar2"
                      }
                    }
                  }
                }
              }
            ],
            "discriminator": {
              "propertyName": "$type",
              "mapping": {
                "bar1": "#/components/schemas/BarBar1",
                "bar2": "#/components/schemas/BarBar2"
              }
            }
          }
        }
      },
      "FooFoo2": {
        "required": [
          "bar"
        ],
        "properties": {
          "$type": {
            "enum": [
              "foo2"
            ],
            "type": "string"
          },
          "musketeer": {
            "type": "integer",
            "format": "int32",
            "default": 3
          },
          "bar": {
            "required": [
              "$type"
            ],
            "type": "object",
            "anyOf": [
              {
                "properties": {
                  "$type": {
                    "enum": [
                      "bar1"
                    ],
                    "type": "string"
                  },
                  "hi": {
                    "type": "string",
                    "default": "Hi"
                  }
                }
              },
              {
                "required": [
                  "left",
                  "right"
                ],
                "properties": {
                  "$type": {
                    "enum": [
                      "bar2"
                    ],
                    "type": "string"
                  },
                  "left": {
                    "discriminator": {
                      "propertyName": "$type",
                      "mapping": {
                        "bar1": "#/components/schemas/BarBar1",
                        "bar2": "#/components/schemas/BarBar2"
                      }
                    }
                  },
                  "right": {
                    "discriminator": {
                      "propertyName": "$type",
                      "mapping": {
                        "bar1": "#/components/schemas/BarBar1",
                        "bar2": "#/components/schemas/BarBar2"
                      }
                    }
                  }
                }
              }
            ],
            "discriminator": {
              "propertyName": "$type",
              "mapping": {
                "bar1": "#/components/schemas/BarBar1",
                "bar2": "#/components/schemas/BarBar2"
              }
            }
          }
        }
      }
    }
  },
  "tags": [
    {
      "name": "Issue"
    }
  ]
}
```
</details>

<details>
<summary>With a dummy endpoint to force resolution of Bar, the correct schema is generated.</summary>

This can be generated by running the `httpsFixed` profile.

```json
{
  "openapi": "3.0.1",
  "info": {
    "title": "Issue | v1",
    "version": "1.0.0"
  },
  "servers": [
    {
      "url": "https://localhost:7242/"
    }
  ],
  "paths": {
    "/foo": {
      "get": {
        "tags": [
          "Issue"
        ],
        "operationId": "GetFoo",
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Foo"
                }
              }
            }
          }
        }
      }
    },
    "/bar": {
      "get": {
        "tags": [
          "Issue"
        ],
        "operationId": "GetBar",
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Bar"
                }
              }
            }
          }
        }
      }
    }
  },
  "components": {
    "schemas": {
      "Bar": {
        "required": [
          "$type"
        ],
        "type": "object",
        "anyOf": [
          {
            "$ref": "#/components/schemas/BarBar1"
          },
          {
            "$ref": "#/components/schemas/BarBar2"
          }
        ],
        "discriminator": {
          "propertyName": "$type",
          "mapping": {
            "bar1": "#/components/schemas/BarBar1",
            "bar2": "#/components/schemas/BarBar2"
          }
        }
      },
      "BarBar1": {
        "properties": {
          "$type": {
            "enum": [
              "bar1"
            ],
            "type": "string"
          },
          "hi": {
            "type": "string",
            "default": "Hi"
          }
        }
      },
      "BarBar2": {
        "required": [
          "left",
          "right"
        ],
        "properties": {
          "$type": {
            "enum": [
              "bar2"
            ],
            "type": "string"
          },
          "left": {
            "$ref": "#/components/schemas/Bar"
          },
          "right": {
            "discriminator": {
              "propertyName": "$type",
              "mapping": {
                "bar1": "#/components/schemas/BarBar1",
                "bar2": "#/components/schemas/BarBar2"
              }
            }
          }
        }
      },
      "Foo": {
        "required": [
          "$type"
        ],
        "type": "object",
        "anyOf": [
          {
            "$ref": "#/components/schemas/FooFoo1"
          },
          {
            "$ref": "#/components/schemas/FooFoo2"
          }
        ],
        "discriminator": {
          "propertyName": "$type",
          "mapping": {
            "foo1": "#/components/schemas/FooFoo1",
            "foo2": "#/components/schemas/FooFoo2"
          }
        }
      },
      "FooFoo1": {
        "required": [
          "bar"
        ],
        "properties": {
          "$type": {
            "enum": [
              "foo1"
            ],
            "type": "string"
          },
          "haha": {
            "type": "string",
            "default": "boo"
          },
          "bar": {
            "$ref": "#/components/schemas/Bar"
          }
        }
      },
      "FooFoo2": {
        "required": [
          "bar"
        ],
        "properties": {
          "$type": {
            "enum": [
              "foo2"
            ],
            "type": "string"
          },
          "musketeer": {
            "type": "integer",
            "format": "int32",
            "default": 3
          },
          "bar": {
            "$ref": "#/components/schemas/Bar"
          }
        }
      }
    }
  },
  "tags": [
    {
      "name": "Issue"
    }
  ]
}
```
</details>