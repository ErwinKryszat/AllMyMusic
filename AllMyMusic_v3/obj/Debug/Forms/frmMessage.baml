   M S B A M L   `   `   `  ����E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  5   	       AllMyMusic.frmMessage   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ,vwclr-namespace:AllMyMusic.View     5   	   -   5        $	��350��5   	   $	��450��5   	     � Icon$:  4/AllMyMusic;component/Resources/Icons/AllMyMusic.ico��5
   	    � Title 5   	   ��  �� Path$ 	TitleText� � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $+ %../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $  ../Styles/ButtonStyle.xaml?�6%   �� 5      $  ../Styles/BorderStyle.xaml?�6%   %    5      .��5      ]�    PagerBackground#���   6   .+�5   
   �� $	��Top=�6     BorderStyle#��[ 6&   $��23��6K   ._�5      �� -   5        $��infoArea��5      $��12(�5      $
��Bold"�5      $��Center=�5      ��5      �� $ CaptionText�5      5      �� 5   
   $��Bottom=�6   #��[ 6)   $��40��6N   ._�5!      �� -   5        $��
buttonArea��5!       �� Orientation$ 
Horizontal=�5"      $��RightToLeft=�63   .+�5$      �� -   5        $��
cmd_Cancel��5$        ButtonStyle#��[ 5&      $��5q�5'      $��60��5(       �� IsCancel . ��5%      �� $ CancelButtonText���5)      �� $ CancelCommand�5*   )   �� 5,      -   5        $��cmd_OK��5,      #��[ 5.      $��5q�5/      $��60��50       �� 	IsDefault . ��5-      �� $ OkButtonText���51      �� $ 	OkCommand�52   *   55      56      � 59   
   	 � RowDefinitions	 5:      �� 5;      $
��Auto �6!   �� 5<      .+�5?      � $��0��6   
 � ColumnDefinitions
 5@      �� 5A      $
��320* �6'   �� 5B      $	��98* �6'   �� 5C      $
��283* �6'   �� 5D      $��60 �6'   .+�5G      �� $��0��6   $��14(�5I      $
��Bold"�5J      $��Center=�5K      $��Center=�5L      " �� VerticalScrollBarVisibility$
 Auto=�5M       �� TextWrapping$
 Wrap=�5N       �� 
IsReadOnly . $��4��5O   ,   $��0,0,59.333,0q�6@   ��5H      �� $ MessageText�.��5O   W   5P      �� 5Q      $��40��6   $��40��6#   $��9.667,10,10.333,10q�6/   $��3��6K   ��6[   �� $ SeverityImage�5S      �� 5U      $��1��6   $
 Auto=�5W      $
 Wrap=�5X       . ��5V      �� $ ExceptionText�.��5Y   (   5Z      5\      5]      5_      