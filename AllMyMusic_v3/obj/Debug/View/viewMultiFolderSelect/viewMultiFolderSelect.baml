   M S B A M L   `   `   `  ����E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:AllMyMusic.ViewModelAllMyMusic.ViewModel  5      +    %AllMyMusic.View.viewMultiFolderSelect   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1vm"clr-namespace:AllMyMusic.ViewModel     5      .��5
      ]� .+�5   
   R� $��0q�6>   ��6   ��   �� Path$  ComputerNames���5      �� 5       �� 
TargetType" Q�6   .�5      ��  �� Property �@Q�
IsExpanded �� Value 63   �� *
IsExpanded+ �� Mode$ TwoWay=��� 5       �@Q�
IsSelected 63   �� *
IsSelected+$ TwoWay=��� 5       � ��$ Normal"�63    �� Triggers 5      P� 5       P� Property �@Q�
IsSelected P� Value . . �5      ��  � ��$
 Bold"�6;   5      5       � 	Resources 5      	 �� DataType,   &AllMyMusic.ViewModel.ComputerViewModel
 �� ItemsSource �� Orientation)   #AllMyMusic.ViewModel.DriveViewModel �� Binding �� Value*   $AllMyMusic.ViewModel.FolderViewModel d� 	IsChecked��%#  (��       5      *, +)(��     5$      *, +)(�� @    5C      *, +)�� 5      "	  5      
 5      �� *
Children+.R�5      �� $ 
Horizontal=�6!   .+�5      �� $��32��6    $��32��6+   $	��3,0q�67   $��images/Computer.png��6D   �� 5      $��Center=�6:   ��6$   �� *Name+6:   5       5!      �� 5#      "	  5$      
 5%      �� *
Children+.R�5&      �� $ 
Horizontal=�6!   .+�5(      �� $��32��6    $��32��6+   $	��3,0q�67   ��5)      �� 5*   "   " ��6(    5+   &   �� 5,   *    66   �� *	DriveType+	 Fixed6T   .a�5-   .   ��  � ��$ images/Hard_Drive.png��6G   5.   +   �� 5/   *    66   �� *	DriveType+	 CDRom6T   .a�50   .   ��  � ��$ images/CD_Drive.png��6G   51   +   �� 52   *    66   �� *	DriveType+ Network6T   .a�53   .   ��  � ��$ images/Network_Drive.png��6G   54   +   �� 55   *    66   �� *DriveIsReady+	 false6W   .a�56   .   ��  �@J�Opacity$	 0.5Q�6H   57   +   59   #   5;      �� 5=      $��Center=�6?   $��10,0,0,0q�6Z   ��6$   �� *	LabelName+6Z   5?      5@      �� 5B      "	  5C      
 5D      �� *
Children+.R�5E      �� $ 
Horizontal=�6!   .+�5G      �� $��16��6    $��16��6+   $	��0,3q�67   ��5H      �� 5I   "   " ��6(    5J   &   �� 5K   *    66   �� *
IsExpanded+ true6U   .a�5L   .   ��  � ��$$ images/FolderOpen_16x16_72.png��6G   5M   +   �� 5N   *    66   �� *
IsExpanded+	 false6U   .a�5O   .   ��  � ��$ images/Folder_16x16.png��6G   5P   +   5R   #   5T      �� 5V      $��Center=�6#   $��10,0,0,0q�6?    6Q   �� *	IsChecked+�� 5X      $��Center=�6:   $��10,0,0,0q�6U   ��6$   �� *Name+6U   5Y      5Z      5[      5\      5^      5_      