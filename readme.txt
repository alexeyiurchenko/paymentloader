
завантажувач файлів DBF для клієнтбанку.


(можна отримати колекцію запитів для postman https://www.getpostman.com/collections/4d0919158edc5f04b3b3)
функціонал
1 - завантажує з папки файли вказаного формату (xls) в БД
2 - відображає завантажений файл по строках у вигляді призначення для платежів (http://localhost:8001/Payment/GetPaymentsData?FileDateFrom=2019-01-01&FileDateTo=2020-01-01)
3 - надано можливість редагувати статус передплатіжних документів (http://localhost:8001/Payment/SetPaymentsData?RowId=102&Accepted=False)
4 - надано можливість вибору рахунку і шаблону для завантаження платежів (http://localhost:8001/Payment/Getaccountsdata)
5 - вибравшни необхідні строки можна сформувати платежі, попередньо вказавши номер рахунку і шаблону з випадаючого списку
(POST /Payment/CreatePayments?templateId=1 HTTP/1.1
Host: localhost:8001
Content-Type: application/json
Cache-Control: no-cache
[{"ID":102},{"ID":101}])
6 - переглянути сформовані, але ще не вивантажені в файл платежі (http://localhost:8001/Payment/GetExportData)
7 - за потреби змінити призначення платежу (http://localhost:8001/Payment/SetExportData?RowId=14&Narrative=за холодну воду 2019.01.09)
8 - по вибраним платежам сформувати і зберегти dbf файл, попередньо вказавши назву файлу (від 4 до 8 літер)
(POST /Payment/ExportToDBF?FileName=1701file HTTP/1.1
Host: localhost:8001
Content-Type: application/json
Cache-Control: no-cache
[{"ID":14},{"ID":15}])
файл зберігається на сервері і одразу завантажується клієнту
9  - переглянути вже сформовані і збережені в dbf файл платежі (http://localhost:50048/Payment/GetEXPORTEDData)
10 - завантажити файл можна за посиланням(http://localhost:8001/upload/uploadfile) або через інтерфейс 
11 - перевести строку завантаженого файлу в статус оброблено - 
(POST Payment/SetPaymentsDataConfirmed HTTP/1.1
Host: localhost:8001
Content-Type: application/json
Cache-Control: no-cache
[{"ID":102},{"ID":101}])

12 - перевести сформований, але не вивантажений в файл, платеіж в статус оброблено - 
(POST Payment/Confirmed HTTP/1.1
Host: localhost:8001
Content-Type: application/json
Cache-Control: no-cache
[{"ID":102},{"ID":101}])

13 - додати шаблон платежу - 
(POST Payment/Confirmed HTTP/1.1
Host: localhost:8001
Content-Type: application/json
Cache-Control: no-cache
{Id:0,
Account:2600,
MFO:215478,
Name:"name2",
EDRPO:"3150510191",
Narrative:"Narrativetext"
})

14 - редагувати шаблон платежу - 
(POST Payment/Confirmed HTTP/1.1
Host: localhost:8001
Content-Type: application/json
Cache-Control: no-cache
{Id:2,
Account:2600,
MFO:215478,
Name:"name2",
EDRPO:"3150510191",
Narrative:"Narrativetext"
})




версійність


---------------------------------------------------------------------------------------------------------------
0.004
- додано глобальний перехоплювач помилок
- пофікшено проблему CORS (але це не точно)
- додано методи редагування/додавання шаблонів платежів
- додано методи переводу завантажених строк в статус оброблено(для виключення їх з відображення)
- додано методи переводу платежів в статус оброблено(для виключення їх з відображення)

---------------------------------------------------------------------------------------------------------------
0.003 
- прибрано кеш для lazyloading EF
---------------------------------------------------------------------------------------------------------------
0.002 
- додано формвання платежів
- додано редагування платежів
- додано збереження всього в файл dbf(c:\inetpub\payments_ob\files\*.dbf)
---------------------------------------------------------------------------------------------------------------
0.001 - mvp
- завантажує файли в бд(приклад файлу c:\inetpub\payments_ob\files\report_example.xls)
- відображає завантажене