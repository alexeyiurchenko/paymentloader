
������������ ����� DBF ��� �볺�������.


(����� �������� �������� ������ ��� postman https://www.getpostman.com/collections/4d0919158edc5f04b3b3)
����������
1 - ��������� � ����� ����� ��������� ������� (xls) � ��
2 - �������� ������������ ���� �� ������� � ������ ����������� ��� ������� (http://localhost:8001/Payment/GetPaymentsData?FileDateFrom=2019-01-01&FileDateTo=2020-01-01)
3 - ������ ��������� ���������� ������ ������������� ��������� (http://localhost:8001/Payment/SetPaymentsData?RowId=102&Accepted=False)
4 - ������ ��������� ������ ������� � ������� ��� ������������ ������� (http://localhost:8001/Payment/Getaccountsdata)
5 - ��������� �������� ������ ����� ���������� ������, ���������� �������� ����� ������� � ������� � ����������� ������
(POST /Payment/CreatePayments?templateId=1 HTTP/1.1
Host: localhost:8001
Content-Type: application/json
Cache-Control: no-cache
[{"ID":102},{"ID":101}])
6 - ����������� ���������, ��� �� �� ���������� � ���� ������ (http://localhost:8001/Payment/GetExportData)
7 - �� ������� ������ ����������� ������� (http://localhost:8001/Payment/SetExportData?RowId=14&Narrative=�� ������� ���� 2019.01.09)
8 - �� �������� �������� ���������� � �������� dbf ����, ���������� �������� ����� ����� (�� 4 �� 8 ����)
(POST /Payment/ExportToDBF?FileName=1701file HTTP/1.1
Host: localhost:8001
Content-Type: application/json
Cache-Control: no-cache
[{"ID":14},{"ID":15}])
���� ���������� �� ������ � ������ ������������� �볺���
9  - ����������� ��� ��������� � �������� � dbf ���� ������ (http://localhost:50048/Payment/GetEXPORTEDData)
10 - ����������� ���� ����� �� ����������(http://localhost:8001/upload/uploadfile) ��� ����� ��������� 
11 - ��������� ������ ������������� ����� � ������ ��������� - 
(POST Payment/SetPaymentsDataConfirmed HTTP/1.1
Host: localhost:8001
Content-Type: application/json
Cache-Control: no-cache
[{"ID":102},{"ID":101}])

12 - ��������� �����������, ��� �� ������������ � ����, ������ � ������ ��������� - 
(POST Payment/Confirmed HTTP/1.1
Host: localhost:8001
Content-Type: application/json
Cache-Control: no-cache
[{"ID":102},{"ID":101}])

13 - ������ ������ ������� - 
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

14 - ���������� ������ ������� - 
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




���������


---------------------------------------------------------------------------------------------------------------
0.004
- ������ ���������� ������������ �������
- ��������� �������� CORS (��� �� �� �����)
- ������ ������ �����������/��������� ������� �������
- ������ ������ �������� ������������ ����� � ������ ���������(��� ���������� �� � �����������)
- ������ ������ �������� ������� � ������ ���������(��� ���������� �� � �����������)

---------------------------------------------------------------------------------------------------------------
0.003 
- �������� ��� ��� lazyloading EF
---------------------------------------------------------------------------------------------------------------
0.002 
- ������ ��������� �������
- ������ ����������� �������
- ������ ���������� ������ � ���� dbf(c:\inetpub\payments_ob\files\*.dbf)
---------------------------------------------------------------------------------------------------------------
0.001 - mvp
- ��������� ����� � ��(������� ����� c:\inetpub\payments_ob\files\report_example.xls)
- �������� �����������