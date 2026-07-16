# Read, fill and flatten AcroForms

Programmatically fill PDF forms — applications, contracts, government forms — and
flatten them so the values become permanent page content.

```csharp
using RedPdf.Manipulation;

// See what's in the form: field names → current values
IReadOnlyDictionary<string, string?> fields = PdfForms.ReadFields(pdf);

// Fill text fields and checkboxes ("Yes"/"On"/"true" check a checkbox)
byte[] filled = PdfForms.FillFields(pdf, new Dictionary<string, string>
{
    ["applicant_name"] = "Ada Lovelace",
    ["subscribe"] = "Yes",
});

// Optionally lock the fields against further edits
byte[] locked = PdfForms.FillFields(pdf, values, makeReadOnly: true);

// Flatten: draw the values into the page content and remove the form entirely
byte[] flat = PdfForms.Flatten(filled);
```

## Notes

- Flattened output prints and archives exactly as filled — nothing is editable anymore.
- Field names are case-sensitive; use `ReadFields` to discover them rather than
  guessing.
- All free Community features.
