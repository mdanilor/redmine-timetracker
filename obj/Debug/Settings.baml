   M S B A M L   `   `   `  ����?  ;FTTW, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:FTTWFTTW  5   	       FTTW.Settings   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:FTTW     5   	     � 
ResizeMode$  NoResize=�5   	    � Title$ Configurações��5	   	   $	��220��6   $	��440��6,   .��5
      � $��20q�6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��35 �6   .+�5   
   �� $��0��6   $��Center=�6!   Id de atividade6M   �� 5   
   $��1��6   $��Center=�6!   $"Tempo de tolerância a inatividade6_   �� 5   
   $��2��6   $��Center=�6!   Ação ao fechar6L   �� 5   
   -   5        $��txtActivityId��5   :   $��0��6   $��1��6   $��8q�6/   6Q   �� 5   
   -   5        $&�� txtIdleSecondsBeforeStopTracking��5   :   $��1��6   $��1��6   $��8q�6/   6d   �� 5   
   -   5        $��cbbCloseAction��5   _   $��2��6   $��1��6    $��Center=�60   $��
8, 0, 8, 0q�6K   @�5      �� 5      .��6   Sempre perguntar5      �� 5      .��6   Minimizar para a bandeja5!      �� 5"      .��6   Fechar5$      5&      �� 5'   
   -   5        $��btnSave��5'   e   $��3��6   $��1��6   $��Right=�6.   $��Center=�6J   $��0, 0, 10, 0q�6�   .��6�   Salvar6�   5(      5)      