   M S B A M L   `   `   `  ����   QuickChargeDisplay*    $QuickChargeDisplay.SetupPasswordPage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     N� Title$  SetupPasswordPage�� N� 
Background ��  �� ImageSource$8 2image\10_관리자화면\관리자기본배경.png��.���� -    ��Name$ 
Compulsion��.+��� -    ��Name$ logOut��$'��!image/20191018_Add/back_12955.png�� �� Left$	 719��
 �� Top$ 386.347��$��50��$��59.47���� $��Right=�$
��Bold"�$��22(�$ -8��$	 103��$��224.089��$��38.16��$��Current Password :�����	#FFF5EDED�� $��Right=�$
��Bold"�$��22(�$ -6��$	 145��$��224.089��$��38.819��$��New Password :�����	#FFF5EDED�� $��Right=�$
��Bold"�$��22(�$ -6��$	 186��$��224.089��$��38.819��$��Confirm Password :�����	#FFF5EDED�� -    ��Name$ btnUpdatePassword��$��Malgun Gothic)�$��20(�$��Normal"�
���   �$ 71��$	 253��$��53��$	��301��$��Admin Pass Change���� -    ��Name$ rectCurPwadmin��
������ $	 222��$	 100��$��40��	 K� ZIndex$	 2��$	��151���� -   $ rectNewPwadmin��
������ $	 221��$	 147��$��40��$	 2��$	��152���� -   $ rectConfirmPwadmin��
������ $	 223��$	 194��$��40��$	 2��$��150.554��G� -   
 G�Name$
 curPw��$��30(�$��Center=�$	 222��$	 102��$��40��$��150.554����Z� G� -	   $
 newPw��$��30(�$��Center=�$	 222��$	 148��$��40��$	��150����Z� G� -
   $
 	confirmPw��$��30(�$��Center=�$	 221��$	 194��$��40��$��150.554����Z� �� -   $
��Lab1��$��Right=�$
��Bold"�$��22(�$	 391��$	 101��$��224.089��$��38.16��$��Current Password :�����	#FFF5EDED�� -   $
��Lab2��$��Right=�$
��Bold"�$��22(�$	 393��$	 144��$��224.089��$��38.819��$��New Password :�����	#FFF5EDED�� -   $
��Lab3��$��Right=�$
��Bold"�$��22(�$	 393��$	 184��$��224.089��$��38.819��$��Confirm Password :�����	#FFF5EDED�� -   $ btnUpdatePassword_Copy��$��Malgun Gothic)�$��20(�$��Normal"�
���   �$	 478��$	 253��$��53��$	��294��$��Manu Pass Change���� -   $ rectCurPwmanual��
������ $	 621��$ 99��$��40��$	 2��$��150.554���� -   $ rectNewPwmanual��
������ $	 621��$	 145��$��40��$	 2��$	��150���� -   $ rectConfirmPwmanual��
������ $	 621��$	 194��$��40��$	 2��$��150.554��G� -   $
 curPwmanual��$��30(�$��Center=�$	 620��$	 100��$��40��$��150.554����Z� G� -   $
 newPwmanual��$��30(�$��Center=�$	 620��$	 144��$��40��$��150.554����Z� G� -   $
 confirmPwmanual��$��30(�$��Center=�$	 620��$	 192��$��40��$��150.554����Z� �� -   $��CahrgeHistory��$	 241��$ 0��$��80��$9��3image\10_관리자화면\아이콘/충전이력.png��$��79���� -   $��TroubleHistory��$	 318��$ 0��$��80��$9��3image\10_관리자화면\아이콘/에러이력.png��$��79���� -   $��LogSampling��$	 720��$ 1��$��80��$9��3image\10_관리자화면\아이콘/로그추출.png��$��79���� -   $��SetupMaxium��$	 562��$ 1��$��78��$?��9image\10_관리자화면\아이콘/최대전류설정.png��$��77���� -   $��TroubleDetails��$ 0��$ 1��$��80��$1��+image/RAPIDAS-R_Monitor/07/관리설정.png��$��78���� -   $��SetupEquipment��$	 478��$ 0��$��80��$7��1image/RAPIDAS-R_Monitor/07/수전전력설정.png��$��82���� -   $��AdjustUnitPrice��$ 79��$ 0��$��80�� J� RenderTransformOrigin$ 0.5,0.5&�$1��+image/RAPIDAS-R_Monitor/07/일시정지.png��$��82���� -   $��
PowerMeter��$	 161��$ 0��$��80��$9��3image\10_관리자화면\아이콘/언어설정.png��$��83���� -   $��EquipmentInfo��$	 639��$ 1��$��80��$9��3image\10_관리자화면\아이콘/강제충전.png��$��80���� -   $��Langanglab1��$ 0��$ 54��$��管理設定��$��27��$��78�����	#FFF2E9E9 � ForceCursor .  $��Stretch=�$��Center=�$��Bottom=��� -   $��Langanglab2��$	 720��$ 55��$��	Log抽出�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -    $��Langanglab3��$ 79��$ 54��$��時間の設定�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -!   $��Langanglab4��$	 161��$ 54��$��言語設定�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -"   $��Langanglab5��$	 242��$ 54��$��充電履歴�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -#   $��Langanglab6��$	 319��$ 53��$��エラー履歴�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -$   $��Langanglab7��$	 398��$ 54��$��パスワード�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -%   $��Langanglab8��$	 478��$ 54��$��受電電力設定�� .  ���	#FFF2E9E9$��27��$��Center=�$��Bottom=�$��81���� -&   $��Langanglab9��$	 562��$ 54��$��電流設定�� .  ���	#FFF2E9E9$��27��$��Center=�$��Bottom=�$��78���� -'   $��Langanglab10��$	 640��$ 55��$��Manual�� .  ���	#FFF2E9E9$��27��$��Center=�$��Bottom=�$��78���� -(   $��Imgmanu��$	 562��$ -1��$��81��$H��B/QuickChargeDisplay;component/image/10_관리자화면/Imgmanu.png��$	��238��$	��Top=� �� StretchDirection$
 Both=��� -)   $��btnUpdateReset��$ 72��$	 321��$��Admin Pass Reset��$��Malgun Gothic)�$��20(�$��Normal"�
���   �$��53��$	��300���� -*   $!��btnUpdatePassword_CopyReset��$	 478��$	 322��$��Manu Pass Reset��$��Malgun Gothic)�$��20(�$��Normal"�
���   �$��53��$	��294��