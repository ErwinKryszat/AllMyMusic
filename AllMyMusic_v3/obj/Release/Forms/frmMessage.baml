   M S B A M L   `   `   `  ���� E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View      AllMyMusic.frmMessage   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ,vwclr-namespace:AllMyMusic.View     -   $	��350��$	��450��  � Icon$:  4/AllMyMusic;component/Resources/Icons/AllMyMusic.ico�� � Title ��  �� Path$ 	TitleText� � 	Resources  �� MergedDictionaries �� Source��  �� $+ %../ResourceDictionaries/MainSkin.xaml?��� $  ../Styles/ButtonStyle.xaml?��� $  ../Styles/BorderStyle.xaml?�%    .��]�    PagerBackground#���   .+��� $	��Top=�  BorderStyle#��[ $��23��._��� -   $��infoArea��$��12(�$
��Bold"�$��Center=����� $ CaptionText��� $��Bottom=�#��[ $��40��._��� -   $��
buttonArea�� �� Orientation$ 
Horizontal=�$��RightToLeft=�.+��� -   $��
cmd_Cancel��  ButtonStyle#��[ $��5q�$��60�� �� IsCancel . ���� $ CancelButtonText����� $ CancelCommand��� -   $��cmd_OK��#��[ $��5q�$��60�� �� 	IsDefault . ���� $ OkButtonText����� $ 	OkCommand�� 	 � RowDefinitions	 �� $
��Auto ��� .+�� $��0��
 � ColumnDefinitions
 �� $
��320* ��� $	��98* ��� $
��283* ��� $��60 �.+��� $��0��$��14(�$
��Bold"�$��Center=�$��Center=�" �� VerticalScrollBarVisibility$
 Auto=� �� TextWrapping$
 Wrap=� �� 
IsReadOnly . $��4��$��0,0,59.333,0q����� $ MessageText�.���� $��40��$��40��$��9.667,10,10.333,10q�$��3������ $ SeverityImage��� $��1��$
 Auto=�$
 Wrap=� . ���� $ ExceptionText�.��