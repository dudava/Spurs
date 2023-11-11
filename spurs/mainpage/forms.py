from django import forms


class CalculateForm(forms.Form):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        for field in self.visible_fields():
            field.field.widget.attrs["class"] = "form-control"

    column_width = forms.FloatField(label="Ширина колонны")
    column_length = forms.FloatField(label="Длина колонны")
    concrete_grade = forms.CharField(label="Марка бетона")
    type_of_I_beam = forms.CharField(label="Тип двутавра")
    hox = forms.FloatField(label="Hox")
    hoy = forms.FloatField(label="Hoy")
    yb1 = forms.FloatField(label="γb1")
    yb3 = forms.FloatField(label="γb3")
    amount_concrete_gravy = forms.FloatField(label="Величина бетонно подливки в метрах")
    shear_force = forms.FloatField(label="Сдвигающее усилие в тоннах")
    value_sealing_spur = forms.CharField(label="Величина заделки шпоры в метрах")

class CalculateChangeForm(forms.Form):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        for field in self.visible_fields():
            field.field.widget.attrs["class"] = "form-check"
            
    change_type_of_I_beam = forms.BooleanField(
        label="Изменять тип двутавра в процесс расчётов?",
        required=False,
    )
    change_value_sealing_spur = forms.BooleanField(
        label="Изменять величину заделки шпоры в метрах в процессе расчётов?",
        required=False,
    )
    