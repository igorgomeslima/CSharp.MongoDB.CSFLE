# CSharp.MongoDB.CSFLE

This sample project can simulate scenery described [here](https://www.mongodb.com/community/forums/t/csfle-problem-when-try-update-array-field-filtering-field-by-the-same-field-name-present-in-csfle-schema-map/179638).

## Edit

To run properly we need to change the section `environmentVariables` in `launchsettings.json` file:

```
...
"environmentVariables": {
   ...
   "CMK_BASE64" : "<PUT HERE YOUR CMK IN BASE64 FORMAT>",
   "MONGODB_CONNECTION_STRING" : "<PUT HERE YOUR MONGODB CONNECTION STRING>"
}
....
```
