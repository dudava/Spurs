from django import forms


class CalculateForm(forms.Form):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        for field in self.visible_fields():
            field.field.widget.attrs["class"] = "form-control"

    column_width = forms.FloatField(label="Ширина колонны (в метрах)")
    column_length = forms.FloatField(label="Длина колонны (в метрах)")
    concrete_grade = forms.ChoiceField(
        label="Класс бетона",
        choices=[
            ("B20", "B20"),
            ("B25", "B25"),
            ("B30", "B30"),
            ("B35", "B35"),
            ("B40", "B40"),
        ]
    )
    type_I_beam = forms.ChoiceField(
        label="Тип двутавра",
        choices=[
            ("15K2", "15K2"),
            ("20K2", "20K2"),
            ("25K2", "25K2"),
            ("30K2", "30K2"),
            ("35K2", "35K2"),
        ],
    )
    # 
    hox = forms.FloatField(label="Hox")
    hoy = forms.FloatField(label="Hoy")
    yb1 = forms.ChoiceField(
        label="γb1",
        choices=[
            (1.0, "При действии всех нагрузок"),
            (0.9, "Только постоянных (ячеистый бетон)"),
            (0.85, "Только постоянных (поризованный бетон)"),
        ]
    )
    amount_concrete_gravy = forms.FloatField(label="Величина бетонной подливки (в метрах)")
    shear_force = forms.FloatField(label="Сдвигающее усилие в тоннах")
    value_sealing_spur = forms.FloatField(label="Величина заделки шпоры (в метрах)")

class CalculateChangeForm(forms.Form):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        for field in self.visible_fields():
            field.field.widget.attrs["class"] = "form-check"
    
    change_type_I_beam = forms.BooleanField(
        label="Изменять тип двутавра?",
        required=False,
    )
    change_value_sealing_spur = forms.BooleanField(
        label="Изменять величину заделки шпоры в метрах?",
        required=False,
    )
        