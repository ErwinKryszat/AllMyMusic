   M S B A M L   `   `   `  ����   
AllMyMusic    AllMyMusic.View.viewPager   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       � 	Resources   �� MergedDictionaries �� Source   pageNumberVisibility��  �� $+ %../ResourceDictionaries/MainSkin.xaml?��� $&  ../Styles/PagerButtonImages.xaml?��� $  ../Styles/ButtonStyle.xaml?�%   &	        �� .��� $��30��  PagerBackground#���   � ColumnDefinitions �� $��* ��� $
��Auto ��� $��* �.+��� 
�������$��1q�
���   � �� CornerRadius$ 15��$��0��$	��130��$
��Left=�y���  �� Path$ ShowPageNumber� �� 	Converter# [  ._��� $��Center=����� $ 
LabelPages��� 
�������$��1q�
���   �$ 15��$��2��$	��130��$��Right=�._��� $��Center=����� $ 
LabelItems���  �� Orientation$ 
Horizontal=�$��1��$��Center=�$	��140��.+��� -    ��Name$ cmdFirstPage��  firstPageImage#��[ $��2q�  ButtonStyle#��[ ���� $ FirstPageCommand��� -   $ cmdPreviousPage��  prevPageImage#��[ $��2q�#��[ ���� $ PrevPageCommand��� -   $ cmdNextPage��  nextPageImage#��[ $��2q�#��[ ���� $ NextPageCommand��� -   $ cmdLastPage��  lastPageImage#��[ $��2q�#��[ ���� $ LastPageCommand�d� -   	 d�Name$	 
cmdShowAll��  showAllImage#��[ $��2q�  ToggleButtonStyle#��[ ���� $ ShowAllCommand�