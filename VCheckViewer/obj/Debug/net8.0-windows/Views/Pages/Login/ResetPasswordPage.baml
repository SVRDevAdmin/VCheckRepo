   M S B A M L   `   `   `  ����G  CVCheckViewer, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullO,clr-namespace:VCheckViewer.Views.Pages.LoginVCheckViewer.Views.Pages.Login  5      6    0VCheckViewer.Views.Pages.Login.ResetPasswordPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >local,clr-namespace:VCheckViewer.Views.Pages.Login     5        N� Title$  VCheck Viewer��5      .��5
      � $��	30,0,30,0q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0��6    �� Padding$ 10,0,0,0q�6.   ._�5      �� $*��$../../../Content/Images/bio_logo.png��6   $	��150��6B   $
��Left=�6N   5      �� 5   
   $��1��6   $��0��6   $ 
10,0,10,20q�6.   ._�5      �  5      �� 5      $��* �6$   �� 5      $
��auto �6$   �� 5      $
��auto �6$   .+�5      �� $-��'../../../Content/Images/Login Photo.png��6   $��0��6I   $	��Top=�6V   �� 5      $#��Get accurate results swiftly.��6   $��1��6A    �� TextAlignment$ Center=�6N   $��Medium"�6e   $��40(�6y   $	��300��6�   $
��Wrap=�6�   $	��Top=�6�   �� 5      $)��#Please login to access your account��6   $��2��6G   $ Center=�6T   $��Medium"�6k   $��25(�6   $��0,0,0,50q�6�   5       5!      �� 5"   
   $��1��6   $ 
50,0,50,20q�6   $��1��63   ._�5#      �  5$      �� 5%      $
��auto �6$   �� 5&      $��* �6$   .+�5(      �� $��1q�6   
�������
������� �� CornerRadius$ 7��6X   $��1��6i   $ 0,60,0,0q�6v   ._�5)      �� $��Center=�6!   $	��350��6>   .+�5*      �� $��Create New Password��6$   $ Center=�6?   $��40(�6V   $
��Bold"�6d   $��0,0,0,0q�6v   $
��Wrap=�6�   �� 5+      -   5         ��Name$ 	ErrorText��5+   $   
���  ��$��Wrong Password��6H   $ Center=�6^   $y�Hidden=�6u   $
��Wrap=�6�   �� 5,      
�������$ 20��64   $ 5q�6F   $��1q�6R   
���   �$��	0,30,0,10q�6z   ._�5-      � .+�5/   "   �� $,��&../../../Content/Images/Email Icon.png��6(   $
��Left=�6X   $��22��6s   $��5,0,0,0q�6   �� 52   "   -   5         ��Name$ Email��52   *   $��0q�69   $��	30,0,10,0q�6M   $��Center=�6`   
���   �6{   �� 53   "    J� IsHitTestVisible$ False��6,   $��Email Address��6E   $��Center=�6Z   $
��Left=�6u   $��	40,0,10,0q�6�   
���������54   &   �� 55   *   	 �� 
TargetType"	 ��60   .�56   .   �� 
 �� Property
 � y� �� Value$ 	Collapsed=�6K    �� Triggers 57   .   �� 58   2    �� Binding 6>   �� *Text+ �� ElementName$ Email�� �� Value  6j   .a�59   6   �� 
 � y�$ Visible=�6S   5:   3   5<   +   5>   #   5?      5@      � 5A       5B      �� 5C   "   $��* �63   �� 5D   "   $
��auto �63   .+�5F      �� 
�������$ 7��68   $ 5q�6I   $��1q�6U   
���   �$��	0,10,0,10q�6}   ._�5G   "   � .+�5I   &   �� $/��)../../../Content/Images/Password Icon.png��6,   $
��Left=�6_   $��30��6z   $��5,-5,0,0q�6�   G� 5L   &   -   5         G�Name$ NewPassword��5L   2   $��0q�6G   $��	30,0,10,0q�6[   $��Center=�6n   
���   �6�   �� 5M   &   -   5        $ NewPasswordPlaceholder��5M   0   $ False��6P   $��New Password��6i   $��Center=�6}   $
��Left=�6�   $��	40,0,10,0q�6�   
���������5N   *   �� 5O   .   "	 ��64   .�5P   2   �� 
 � y�$ Visible=�6O   5Q   /   5S   '   5T   #   5U      �� 5V      -   5        $ NewPasswordLength��5V   (   $*��$Must be at least 10 characters longs��6C   $��10(�6o   $��	15,0,0,-5q�6}   
���  ��$��Bottom=�6�   $y�Hidden=�6�   �� 5W      $��1��6&   $��	0,0,-30,0q�66   ._�5X   "   �� -   5         ��Name$ NewpasswordCorrect��5X   (   $+��%../../../Content/Images/path_4002.png��6D   $��20��6s   $y�Hidden=�6~   5Y      5Z      � 5[       5\      �� 5]   "   $��* �63   �� 5^   "   $
��auto �63   .+�5`      �� 
�������$ 20��68   $ 5q�6J   $��1q�6V   
���   �$��	0,10,0,30q�6~   ._�5a   "   � .+�5c   &   �� $/��)../../../Content/Images/Password Icon.png��6,   $
��Left=�6_   $��30��6z   $��5,-5,0,0q�6�   G� 5f   &   -   5        $ ConfirmPassword��5f   2   $��0q�6K   $��	30,0,10,0q�6_   $��Center=�6r   
���   �6�   �� 5g   &   -   5        $  ConfirmPasswordPlaceholder��5g   0   $ False��6T   $��Confirm Password��6m   $��Center=�6�   $
��Left=�6�   $��	40,0,10,0q�6�   
���������5h   *   �� 5i   .   "	 ��64   .�5j   2   �� 
 � y�$ Visible=�6O   5k   /   5m   '   5n   #   5o      �� 5p      -	   5        $��ConfirmPasswordLength��5p   (   $*��$Must be at least 10 characters longs��6E   $��10(�6q   $��	15,0,0,15q�6   
���  ��$��Bottom=�6�   $y�Hidden=�6�   �� 5q      $��1��6&   $��0,-20,-30,0q�66   ._�5r   "   �� -
   5        $ ConfirmPasswordCorrect��5r   (   $+��%../../../Content/Images/path_4002.png��6H   $��20��6w   $y�Hidden=�6�   5s      5t      �� 5u      $��Reset Password��6!      RoundButton#���   6:   
��� ���
�������$��Stretch=�6�   $��40��6�   $��	0,20,0,15q�6�   �� 5v      -   5        $��Back to login��5v   !   
���   �$
��Left=�6L   
������ $��0q�6�   6�   5w      5x      �� 5z      $��0��6   ._�5{      �� $+��%../../../Content/Images/Cat Photo.png��6   $	��120��6K   $��	0,0,0,-40q�6X   5|      5}      5~      5�      5�      