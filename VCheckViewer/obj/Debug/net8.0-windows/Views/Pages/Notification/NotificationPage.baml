   M S B A M L   `   `   `  ����G  CVCheckViewer, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null]3clr-namespace:VCheckViewer.Views.Pages.Notification%VCheckViewer.Views.Pages.Notification  5      E'clr-namespace:VCheckViewer.UserControlsVCheckViewer.UserControls  6{   <    6VCheckViewer.Views.Pages.Notification.NotificationPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Elocal3clr-namespace:VCheckViewer.Views.Pages.Notification     5      A =Wpf.Ui, Version=3.0.5.0, Culture=neutral, PublicKeyToken=null/ui&http://schemas.lepo.co/wpfui/2022/xaml  6I   :userControls'clr-namespace:VCheckViewer.UserControls  6{     N� Title$  NotificationPage��5       � 	Resources 5
         test �� 
TargetType �� Property �� Value  TextBoxStyle   Themes_SearchFieldBackground  Themes_SearchFieldFontColor��%  &	        &	 H     �� 5   
   " ]�6   .�5      ��  � ��$ 80��6'   5      �� 5   
   " ��6%   .�5      ��  � ��# �  6+   �� 5       � ��$ Center=�69   �� 5       � ��# �  6+   �� 5       � ��$ 
5, 2, 0, 0q�6(   5      5      .��5      � -   5        $��	0,10,0,10q�5       � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��0.65* �6   �� 5      $��0.84* �6   �� 5      $
��0.7* �6    � RowDefinitions 5   
   �� 5       $��* �6   �� 5!      $��6* �6   �� 5"      $��* �6   .+�5$   
   �� $��0��6   $��4��6   
������  �� CornerRadius$ 7��6K   $��	10,5,10,5q�6\   6q   �� 5%   
   $��Stretch=�6   $��Center=�6/   $��0��6J   $��	20,0,10,0q�6Z   $��45��6m   ._�5&      �� -   5         % !Themes_NotifSearchfieldBackground#���  5&      $ 7��6V    �� Padding$ 5q�6g   $��1q�6s   #���  6�   ._�5'      � .+�5*      ��   Themes_SearchIcon#���  6   $��24��6I   $
��Left=�6T   $��
5, 2, 0, 0q�6o   �� 5-      -   5        	 ��Name$	 KeywordSearchBar��5-      $��0q�68   $��	30,0,10,0q�6L   $��Center=�6_   #��[ 6�   �� 5.      
 J� IsHitTestVisible$
 False��6    $��Center=�6�   $
��Left=�6�   $��	40,0,10,0q�6�   
���������69   ��  �� Path$ Results_Label_Searchbar�  	Resources �� Source# [ ��5/      �� 50      " ��6$   .�51   "   ��  � y�$ 	Collapsed=�6?    �� Triggers 52   "   �� 53   &    �� Binding 62   �� *Text+ �� ElementName$ KeywordSearchBar�� �� Value  6i   .a�54   *   ��  � y�$ Visible=�6G   55   '   57      59      5:      5;      5<      �� 5=   
   $��Stretch=�6   $��Center=�6/   $��1��6J   $��0,0,0,0q�6Z   ._�5>      �� #���  6   $ 7��6V   $ 5q�6g   $��1q�6s   #���  6�   ._�5?      � .+�5@      ��   Themes_CalendarIcon#���  6   $��24��6K   $
��Left=�6V   $��
5, 2, 0, 0q�6q   /   )VCheckViewer.UserControls.DateRangePicker  5D      -   5          Name$ 	RangeDate��5D   3   $��0q�6�   $��	30,0,10,0q�6�   $��Center=�6�      SelectedDateRange 6F   �� *SelectedDates+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   5E      5F      5G      �� 5H   
   $��2��6   $��45��6!   $ 7��6-   $��1q�6>   #���  6R   #���  6�   $��10,0,0,0q�6�   ._�5]      �� #���  6   $��0q�6V   $ 7��6j   $ 5,0,5,0q�6{   ._�5^      � .+�5`      �� $��15(�6q   $
��Left=�6   $��Center=�6�   $��5,0,0,0q�6�    $  Themes_NotifSearchFieldFontColor#���  6�   ��6    �� $ Notification_Label_Show�# [ 6�   �� 5b      -   5        $��NotificationType��5b      #���  67   #���  6x   $��0q�6�   $��Center=�6�   $��50,0,0,0q�6�   $
��Bold"�6�   .@�5c      �� $��Normal"�6~    �� 
IsSelected$
 True��6�   ��6'   �� $  Notification_Label_ShowAll�# [ 6�   �� 5d      
 � Tag$ Upcoming Test��6�   $��Normal"�6�   ��6'   �� $% Notification_Label_UpcomingTest�# [ 6�   �� 5e      $ Completed Test Results��6�   $��Normal"�6�   ��6'   �� $&  Notification_Label_CompletedTest�# [ 6�   �� 5f      $ Updates��6~   $��Normal"�6�   ��6'   �� $  Notification_Label_Updates�# [ 6�   5g      5h      5i      5j      �� 5k   
   $��Stretch=�6   $��Center=�6/   $��3��6J   $��	10,0,20,0q�6Z   ._�5l      �� -   5        
�������$��Stretch=�5l   �    	 Themes_ButtonColor#��� 	 6�   $��0q�6�    
 RoundButton#��� 
 6�   ��6   �� $% Notification_Label_UpdateFilter�# [ 6  5m      �� 5n   
   $��4��6   $��1��6%   #���  62   $ 7��6s   $��20 10, 20, 10q�6�   $ 70,20,70,30q�6�   ._�5o      �� -   5         ��Name$ NotificationViewList��5o      
������ 5�      5�      -   'VCheckViewer.UserControls.PagingControl  5�   
   -   5          Name$ 
pagination��5�   %   $��2��69   $��0��6F   $��6��6V   5�      5�      5�      