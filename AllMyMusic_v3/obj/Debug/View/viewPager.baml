   M S B A M L   `   `   `  ����  
AllMyMusic    AllMyMusic.View.viewPager   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � 	Resources  5	       �� MergedDictionaries �� Source   pageNumberVisibility�� 5
   
    5      �� 5      $+ %../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $&  ../Styles/PagerButtonImages.xaml?�6%   �� 5      $  ../Styles/ButtonStyle.xaml?�6%   %   &	        �� 5      6+   5      .��5      � $��30��6     PagerBackground#���  6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� 
�������$��1q�5      
���   � �� CornerRadius$ 15��5      $��0��5      $	��130��5       $
��Left=�5!      y�5"      ��  �� Path$ ShowPageNumber� �� 	Converter# [  ._�5#      �� $��Center=�69   ��6   �� $ 
LabelPages�69   5%      �� 5'   
   
�������$��1q�5(      
���   �$ 15��5*      $��2��5+      $	��130��5,      $��Right=�5-      ._�5.      �� $��Center=�69   ��6   �� $ 
LabelItems�69   5/      �� 51   
    �� Orientation$ 
Horizontal=�6   $��1��6.   $��Center=�6>   $	��140��6[   .+�52      �� -   5         ��Name$ cmdFirstPage��52        firstPageImage#��[ 53      $��2q�54        ButtonStyle#��[ 56      ��55      �� $ FirstPageCommand�57      �� 59      -   5        $ cmdPreviousPage��59        prevPageImage#��[ 5:      $��2q�5;      #��[ 5=      ��5<      �� $ PrevPageCommand�5>      �� 5@      -   5        $ cmdNextPage��5@        nextPageImage#��[ 5A      $��2q�5B      #��[ 5D      ��5C      �� $ NextPageCommand�5E      �� 5G      -   5        $ cmdLastPage��5G        lastPageImage#��[ 5H      $��2q�5I      #��[ 5K      ��5J      �� $ LastPageCommand�5L      d� 5N      -   5        	 d�Name$	 
cmdShowAll��5N        showAllImage#��[ 5O      $��2q�5P        ToggleButtonStyle#��[ 5R      ��5Q      �� $ ShowAllCommand�5S      5T      5W      5X      