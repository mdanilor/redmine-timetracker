   M S B A M L   `   `   `  ���� ?  ;FTTW, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:FTTWFTTW      FTTW.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     $localclr-namespace:FTTW       � Title$$  Apontador de horas trabalhadas��$	��550��$	��920��$	��920��$	��390�� � Icon$ icons/clock.png��.���  � RowDefinitions �� $��* ��� $��18 �.+��  � ColumnDefinitions �� $	��240 ��� $��* �.+��� $��0��
�������$��1,1,1,1q�$��10q���Z� �� $��1��
�������$��1,1,1,1q�$��
10,10,10,0q���Z� �  �� $	��150 ��� $��* ��� $	��200 �.+��� $��1��
�������$��0,0,0,1q�$��12q���Z� � $��15q� � Orientation$ Vertical=�$��Center=�.+��� -   $��lblTime��$��0��$��32(� �� TextAlignment$ Center=�$	��Top=�$��6q�
00:00:00�� -   $��btnStartStop��$��Center=����#79DB75���#AAE5AA$��1q�.���� -   $��lblStartStop��$��5q�$��18(�	Iniciar�� -   $��lblWorkingOnIssue��$��0��$��14(�$ Center=�&   System.Windows.Controls.DataGrid  -   $��	dgHistory��   AutoGenerateColumns .     CanUserSortColumns .     
IsReadOnly . 	   GridLinesVisibility$
	 None=�$��0q�
   HeadersVisibility$

 None=�
�������   CanUserResizeColumns .     CanUserResizeRows .     RowHeaderWidth$ 0Q�$��15q�$��Disabled=�$��Disabled=�$��1��   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ 25    CellTemplate �� .R��� $��btnSidePlay��-   
�������
������� �� 	ClickMode$ Press=�.���� $��Center=�$��Center=�.+��� $��22��$��22������  �� 
TargetType" �� �� Triggers ��  �� Binding �� *
IsActive+ �� Value	 false.a���  �� Property � �� �� Value$ icons/start.png����  �� *
IsActive+ true.a���  � ��$ icons/pause.png��0  *System.Windows.Controls.DataGridTextColumn     Header$ Projeto��$ 90 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	Project+  $ Tarefa��$ 45  �� *Task+  $ Tempo��$ 50  �� *Time+� $��2��$ Vertical=�$��Center=�.+��� $��Center=�$ Center=�$��20(�$��4q�Última semana  -   $��
dgLastWeek�� .  $��0q�
���   � . $
	 None=�
������� .   .   .  $ 0Q�$��5q� � 	Resources =  7System.Windows.Controls.Primitives.DataGridColumnHeader��%3   '        �� "  .���  � �� �#EEEEFF   	CellStyle �� *  $System.Windows.Controls.DataGridCell"  .���  � ��
 �������  � ��$ 0q� P�  P� Property �@ 
IsSelected P� Value . . ���  � ��
 �������  � ��
 �   ���  � ��$ 0q�   $	 Dia��$	 100  �� *Day+  $ Horas��$ 60  �� *Time+�� -   $��
cbProjects��$��1��$	��Top=�$
��Left=�$��	20,15,0,0q����� .R��� ���� *Name+����  �� Path$ ForegroundColor��� -	   $��	cbServers��$��1��$	��Top=�$��Right=�$��0,15,315, 0q�$	��150������ .R��� ���� *Name+�� -
    ��Name$ btnAddServer��$��1��$	��Top=�$��Right=�$��
0,15,220,0q�$��Novo servidor��$��4,3,4,2q����#DDEEDD�� -   $ 	btnLogout��$��1��$	��Top=�$��Right=�$��
0,15,110,0q�$��4,3,4,2q����#FFEEEE$��Sair deste servidor���� -   $ btnSettings��$��1��$	��Top=�$��Right=�$��	0,15,20,0q�$��4,3,4,2q�
�������$��Configurações��  -   $��dgTasks��$��1��$��20,40,20,36q� .      	RowHeight$  20Q�!   SelectionMode$! Single=� . $��Disabled=�
������� " �� Color��%A   (��       * SystemColors.HighlightBrushKey+)�� $" #B3DBF2��#   RowStyle# �� )	  #System.Windows.Controls.DataGridRow" 	  ��  �� *
IsActive+ true.a���  � �� �#AAE5AA$   RowDetailsTemplate$ �� .R�� ���#B3DBF2 �� $��* ��� .+��� $��0�����#B3DBF2$��Stretch=�$��Stretch=����� *Description+�� $��btnWeb��-   $��2q�$��1��$
��Left=�$��Center=����#C0E9FF$ Release=�.���� $��Abrir na web��$��
2, 0, 2, 0q�   $ 25  �� .R��� $��btnPlay��-   
�������
�������$ Press=�.���� $��Center=�$��Center=�.+��� $��22��$��22������ " �� ��  �� *
IsActive+	 false.a���  � ��$ icons/start.png����  �� *
IsActive+ true.a���  � ��$ icons/pause.png��  $ Id��$ 30  �� *Id+  $
 Nome��$ *  �� *Name+  $ Tempo��$ 60  �� *
TimeString+�� $��1��$��Right=�$��Bottom=�$��
0,0,155,10q�	Filtro:�� -   $��txtIssueFilter��$��1��$��Right=�$��Bottom=�$��	0,0,20,10q�$	��130���� -   $��btnMinimizeFromFull��$��Right=�$��Center=�$��1��$��0,0,5,0q�
�������
�������.���� .+��� -   $��imgMinimize��$��icons/arrow_left.png��$��18��$��18��