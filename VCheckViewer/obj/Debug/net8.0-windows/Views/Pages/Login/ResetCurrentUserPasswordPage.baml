   M S B A M L   `   `   `  ����G  CVCheckViewer, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullO,clr-namespace:VCheckViewer.Views.Pages.LoginVCheckViewer.Views.Pages.Login  5      A    ;VCheckViewer.Views.Pages.Login.ResetCurrentUserPasswordPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >local,clr-namespace:VCheckViewer.Views.Pages.Login     5        N� Title$  VCheck Viewer��5      .��5
      � $��	0,10,0,10q�6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ 	ErrorText��5      
���  ��$y�Visible=�68   $
��Wrap=�6M   �� 5   
   $��1��6   $��	0,50,0,40q�6!   ._�5      �� $��30q�6   $��1q�6!    '  #Themes_ResetPasswordFrameBackground#���   65    �� Padding$ 20q�6y    �� CornerRadius$ 7��6�   #���   6�   ._�5      �  � RowDefinitions 5      �� 5      $
��auto �6(   �� 5      $
��auto �6(   �� 5      $
��auto �6(   �� 5      $
��auto �6(   �� 5      $
��auto �6(   �� 5      $
��auto �6(   �� 5      $
��auto �6(   .+�5      ��   Themes_FontColor#���  6q   $��0��6�   $��Bottom=�6�   $��10,20,10,10q�6�   $
��Bold"�6�   ��6    ��  �� Path$ Login_Label_OldPassword�  	Resources �� Source# [ 6�   �� 5      $��1��6   
�������$ 7��6=   $ 5q�6N   $��1q�6Z   
���   �$��0,0,0,20q�6�   ._�5      G� -   5         G�Name$ OldPassword��5   &   $��0q�6;   $��	10,0,10,0q�6O   $��Center=�6b   6}   5       �� 5!      #���  6q   $��2��6�   $��Bottom=�6�   $��10q�6�   $
��Bold"�6�   ��6    �� $ Login_Label_NewPassword�# [ 6�   �� 5"      $��3��6   
�������$ 7��6=   $ 5q�6N   $��1q�6Z   
���   �$��0,0,0,20q�6�   ._�5#      G� -   5        $ NewPassword��5#   &   $��0q�6;   $��	10,0,10,0q�6O   $��Center=�6b   6}   5$      �� 5%      #���  6u   $��4��6�   $��Bottom=�6�   $��10q�6�   $
��Bold"�6�   ��6    �� $! Login_Label_ConfirmPassword�# [ 6�   �� 5&      $��5��6   
�������$ 7��6=   $ 5q�6N   $��1q�6Z   
���   �$��0,0,0,40q�6�   ._�5'      G� -   5        $ ConfirmPassword��5'   &   $��0q�6?   $��	10,0,10,0q�6S   $��Center=�6f   6�   5(      �� 5)      -   5        	 ��Name$	 ResetPassword��5)   *   $��6��6   $��0q�5*      $��0q�61   $��Center=�6<   $��Stretch=�6W   $��40��6u     Themes_ButtonColor#���  5+        Themes_ButtonFontColor#���  6O     RoundButton#���  5,      #���  6C   ${�False��6�   ��5)   A   �� $ Login_Label_ResetPassword�# [ 5,   �   5-      5.      5/      50      51      