   M S B A M L   `   `   `  ���� E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:AllMyMusic.ViewModelAllMyMusic.ViewModel  +    %AllMyMusic.View.viewMultiFolderSelect   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     1vm"clr-namespace:AllMyMusic.ViewModel     .��]� .+�R� $��0q�����   �� Path$  ComputerNames�����  �� 
TargetType" Q�.���  �� Property �@Q�
IsExpanded �� Value �� *
IsExpanded+ �� Mode$ TwoWay=���  �@Q�
IsSelected �� *
IsSelected+$ TwoWay=���  � ��$ Normal"� �� Triggers P�  P� Property �@Q�
IsSelected P� Value . . ���  � ��$
 Bold"� � 	Resources 	 �� DataType,   &AllMyMusic.ViewModel.ComputerViewModel
 �� ItemsSource �� Orientation)   #AllMyMusic.ViewModel.DriveViewModel �� Binding �� Value*   $AllMyMusic.ViewModel.FolderViewModel d� 	IsChecked��%@  (��       *, +)(�� �     *, +)(�� �    *, +)�� "	  
 �� *
Children+.R��� $ 
Horizontal=�.+��� $��32��$��32��$	��3,0q�$��images/Computer.png���� $��Center=����� *Name+�� "	  
 �� *
Children+.R��� $ 
Horizontal=�.+��� $��32��$��32��$	��3,0q����� " �� ��  �� *	DriveType+	 Fixed.a���  � ��$ images/Hard_Drive.png����  �� *	DriveType+	 CDRom.a���  � ��$ images/CD_Drive.png����  �� *	DriveType+ Network.a���  � ��$ images/Network_Drive.png����  �� *DriveIsReady+	 false.a���  �@J�Opacity$	 0.5Q��� $��Center=�$��10,0,0,0q����� *	LabelName+�� "	  
 �� *
Children+.R��� $ 
Horizontal=�.+��� $��16��$��16��$	��0,3q����� " �� ��  �� *
IsExpanded+ true.a���  � ��$$ images/FolderOpen_16x16_72.png����  �� *
IsExpanded+	 false.a���  � ��$ images/Folder_16x16.png���� $��Center=�$��10,0,0,0q� �� *	IsChecked+�� $��Center=�$��10,0,0,0q����� *Name+