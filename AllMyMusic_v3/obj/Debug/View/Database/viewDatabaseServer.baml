   M S B A M L   `   `   `  ����E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  5      (    "AllMyMusic.View.viewDatabaseServer   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,vwclr-namespace:AllMyMusic.View     5        � 	Resources  5
       �� MergedDictionaries �� Source   userNameVisibility1   +AllMyMusic.View.ServerTypeToStringConverter   serverTypeToStringConverter1�� 5   
    5      �� 5      $# ../../Styles/ButtonStyle.xaml?�6%   �� 5      $% ../../Styles/ComboBoxStyle.xaml?�6%   %:   &	        &	      �� 5      6+     5      6-   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5       $	��150��6   �� 5!      $	��150 �6   .+�5$   
   �� $��0��6   $��0��6!   $��5,2,5,2q�61   ��5%      ��  �� Path$ ServerTypes� �� SelectedItem 5&      �� $ SelectedServerType� �� 	Converter# [ �� 5(   
   $��1��6   $��0��6!   $��5,2,5,2q�62   ��5)      �� $ 
ServerName��� 5+   
   $��2��6   $��0��6!   $��5,2,5,2q�62   ��5,      �� $ UserName�y�5-      �� $ ShowUserName�# [  �� 5/   
   $��3��6   $��0��6!   $��5,2,5,2q�62   ��50      �� $ Password�y�51      �� $ ShowUserName�# [  �� 53   
   $��4��6   $��0��6   $��Test Connection��6/   $��5,2,5,2q�6J     ButtonStyle#��[ 55      ��54      �� $ TestConnectionCommand�55      �� 57   
   $��0��6:   $��1��6H   $��5q�6Y   ��6   �� $ Combo_ServerType�6Y   �� 58   
   $��1��6:   $��1��6H   $��5q�6Y   ��6   �� $ Combo_ServerName�6Y   �� 5:   
   $��2��60   $��1��6>   $��5q�6O   ��6   �� $ TbUser�y�5;      �� $ ShowUserName�# [  �� 5=   
   $��3��64   $��1��6B   $��5q�6S   ��6   �� $ 
TbPassword�y�5>      �� $ ShowUserName�# [  5B      5C      