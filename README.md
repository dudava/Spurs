# Приложение для расчёта элементов конструкции

## Запуск Django приложения (на windows)
```
Создание виртуального окружения
py -m venv venv
venv/scripts/activate
pip install -r requirements/prod.txt

Применения миграций и запуск приложения
cd spurs
python manage.py migrate
python manage.py runserver
```