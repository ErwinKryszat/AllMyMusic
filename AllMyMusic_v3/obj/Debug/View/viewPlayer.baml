   M S B A M L   `   `   `  ����H  DAllMyMusic_v3, Version=3.0.0.1, Culture=neutral, PublicKeyToken=null7 clr-namespace:AllMyMusic_v3.ViewAllMyMusic_v3.View  5      ?$clr-namespace:AllMyMusic_v3.ControlsAllMyMusic_v3.Controls  5      #    AllMyMusic_v3.View.viewPlayer   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /vw clr-namespace:AllMyMusic_v3.View     5      9controls$clr-namespace:AllMyMusic_v3.Controls     5        � 	Resources  5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $+ %../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $2 ,../ControlTemplates/AmmSliderHorizontal.xaml?�6%   %    5      .��5      �    PagerBackground#��[  6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5"   
   )   #AllMyMusic_v3.Controls.CustomSlider  $��0��6!   $��2��6.   $��10q�6B    � Minimum$ 0Q�5#   !     templateSliderHorizontal#��[ 5'   !    � Maximum 5$   !   ��  �� Path$ StreamLength� � Value 5%   !   �� $ StreamPosition�	   
IsDragging	 5&   !   �� $ SliderIsDragging�
 �� Mode$
 OneWayToSource=�5'   !   +   %AllMyMusic_v3.View.viewPlayerSongItem  5*   
   $��1��6    $��0��6-   (   "AllMyMusic_v3.Controls.KnobControl  5+   
   $��1��6   $��1��6,   $	��100��6<   $	��100��6H   $	��Top=�6s      Value 6U   �� $ Volume�6s   +   %AllMyMusic_v3.View.viewPlayerCommands  5-   
   $��2��6    $��2��6-   5.      5/      