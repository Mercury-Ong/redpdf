# Create form fields

Add interactive text fields and checkboxes to any PDF — turn a rendered document or a
scan into a fillable form.

```csharp
using RedPdf.Manipulation;

byte[] fillable = PdfForms.AddFields(pdf,
    new TextFieldSpec
    {
        Name = "full_name",
        PageNumber = 1,
        X = 90, Y = 180,          // points from the top-left of the page
        Width = 220, Height = 22,
        Value = "",               // optional preset
        Required = true,
    },
    new TextFieldSpec
    {
        Name = "notes",
        PageNumber = 1,
        X = 90, Y = 220,
        Width = 320, Height = 80,
        Multiline = true,
    },
    new CheckBoxSpec
    {
        Name = "agree_terms",
        PageNumber = 1,
        X = 90, Y = 320,
        Width = 14, Height = 14,
        Checked = false,
    });
```

The result opens as an editable form in any PDF viewer. Fields can be filled by users
or programmatically with [`PdfForms.FillFields`](forms.md), and made permanent with
`PdfForms.Flatten`.

## Notes

- Adding fields to a document that already has a form preserves the existing fields;
  duplicate names are rejected.
- Field names must not contain `.` (reserved for field hierarchies).
- Free Community feature.
