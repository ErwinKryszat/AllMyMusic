   M S B A M L   `   `   `  ����  AllMyMusic_v3(    "AllMyMusic_v3.View.viewNewDatabase   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � 	Resources  5	       �� MergedDictionaries �� Source�� 5
   
    5      �� 5      $+ %../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $  ../Styles/ButtonStyle.xaml?�6%   �� 5      $" ../Styles/ComboBoxStyle.xaml?�6%   %    5      .��5      �    PagerBackground#���   6    � RowDefinitions 5   
   �� 5      �� 5      �� 5       � ColumnDefinitions 5   
   �� 5      $	��150��6   .+�5   
   �� $��0��6   $��5,0,6,1q�6   ��5      ��  �� Path$ CreateDatabaseName� �� UpdateSourceTrigger$ PropertyChanged=��� 5!   
   $��1��6   $��5,0,5,0q�6     �� 
IsEditable .  ��5#      �� $ 
Collations� �� SelectedValue 5$      �� $ CreateDatabaseCollation��� 5&   
   $��2��6   $��5,0,5,0q�5'        ButtonStyle#��[ 5*      ��5(      �� $ CreateDatabaseCommand���5)      �� $ CreateDatabaseName���5+      �� 5,      	 �� Orientation$	 
Horizontal=�6   .+�5-      �� $
��Bold"�6    $��Center=�62   $��0,0,10,0q�6M   NEW6d   �� 5.      $?��9/AllMyMusic_v3;component/Resources/images/database_32.png��6   ��5/      �� 50      
 �� 
TargetType"
 ��6$    �� Triggers 51   "   P� 52   &    P� Property � {� P� Value .  . �53   *   ��  �� Property �@J�Opacity �� Value$	 0.5Q�6D   54   '   56      58      59       � ToolTip 5;      �� 5<      ��6   �� $  Cmd_CreateDatabase_ToolTip�5>      5@      5A      