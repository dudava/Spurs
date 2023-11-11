import django.shortcuts
import requests

from . import forms

def index(request):
    template = "mainpage/index.html"
    calculate_form = forms.CalculateForm(request.POST or None)
    calculate_change_form = forms.CalculateChangeForm(request.POST or None)
    if calculate_form.is_valid() and calculate_change_form.is_valid():
        calculate_data = calculate_form.cleaned_data
        calculate_change_data = calculate_change_form.cleaned_data
        print(calculate_data)
        print(calculate_change_data)
        post_form = {**calculate_data, **calculate_change_data}
        print(post_form)
    
    calculate_form_fields = calculate_form.visible_fields()
    context = {
        "fields_first_col": calculate_form_fields[::2],
        "fields_second_col": calculate_form_fields[1::2],
        "calculate_change_form": calculate_change_form,
    }
        
    return django.shortcuts.render(
        request,
        template_name=template,
        context=context
    )
    