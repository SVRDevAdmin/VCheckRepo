-- Language & Country Setting Update ---------
INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='LC01' LIMIT 1), 
'en', 'Language & Country Setting Update', 'You''ve updated the system language and country settings. Please log in again to apply the changes.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='LC01' LIMIT 1), 
'zh-Hans', '语言和国家/地区设置更新', '您已更新系统语言和国家/地区设置。请重新登录以应用更改。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='LC01' LIMIT 1), 
'zh-Hant', '語言和國家設定更新', '您已更新系統語言和國家設定。請重新登入以套用變更。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='LC01' LIMIT 1), 
'es', 'Actualización de configuración de idioma y país', 'Has actualizado la configuración de idioma y país del sistema. Inicie sesión nuevamente para aplicar los cambios.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='LC01' LIMIT 1), 
'vi', 'Cập nhật cài đặt ngôn ngữ và quốc gia', 'Bạn đã cập nhật cài đặt ngôn ngữ hệ thống và quốc gia. Vui lòng đăng nhập lại để áp dụng các thay đổi.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='LC01' LIMIT 1), 
'hi', 'भाषा और देश सेटिंग अपडेट', 'आपने सिस्टम भाषा और देश सेटिंग अपडेट कर दी है। कृपया परिवर्तन लागू करने के लिए फिर से लॉग इन करें।',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='LC01' LIMIT 1), 
'pt', 'Atualização de configuração de idioma e país', 'Você atualizou as configurações de idioma e país do sistema. Faça login novamente para aplicar as alterações.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='LC01' LIMIT 1), 
'ru', 'Обновление настроек языка и страны', 'Вы обновили настройки языка и страны системы. Пожалуйста, войдите еще раз, чтобы применить изменения.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='LC01' LIMIT 1), 
'ja', '言語と国の設定の更新', 'システムの言語と国の設定が更新されました。変更を適用するには再度ログインしてください。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='LC01' LIMIT 1), 
'de', 'Aktualisierung der Sprach- und Ländereinstellungen', 'Sie haben die Systemsprache und Ländereinstellungen aktualisiert. Bitte melden Sie sich erneut an, um die Änderungen zu übernehmen.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='LC01' LIMIT 1), 
'id', 'Pembaruan Pengaturan Bahasa & Negara', 'Anda telah memperbarui pengaturan bahasa dan negara sistem. Silakan masuk lagi untuk menerapkan perubahan.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='LC01' LIMIT 1), 
'ko', '언어 및 국가 설정 업데이트', '시스템 언어 및 국가 설정을 업데이트했습니다. 변경 사항을 적용하려면 다시 로그인하세요.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='LC01' LIMIT 1), 
'fr', 'Mise à jour des paramètres de langue et de pays', 'Vous avez mis à jour les paramètres de langue et de pays du système. Veuillez vous reconnecter pour appliquer les modifications.',
NOW(), 'SYSTEM');


-- PMS/LIS/HIS Setting Update ------
INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='CS01' LIMIT 1), 
'en', 'PMS/LIS/HIS Setting Update', 'The PMS/LIS/HIS setting has been modified by ###<admin_fullname>### (###<admin_id>###). Please log in again to apply the changes.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='CS01' LIMIT 1), 
'zh-Hans', 'PMS/LIS/HIS 设置更新', 'PMS/LIS/HIS 设置已被 ###<admin_fullname>### (###<admin_id>###) 修改。请重新登录以应用更改。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='CS01' LIMIT 1), 
'zh-Hant', 'PMS/LIS/HIS 設定更新', 'PMS/LIS/HIS 設定已由 ###<admin_fullname>### (###<admin_id>###) 修改。請重新登入以套用變更。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='CS01' LIMIT 1), 
'es', 'Actualización de configuración de PMS/LIS/HIS', 'La configuración de PMS/LIS/HIS ha sido modificada por ###<admin_fullname>### (###<admin_id>###). Inicie sesión nuevamente para aplicar los cambios.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='CS01' LIMIT 1), 
'vi', 'Cập nhật cài đặt PMS/LIS/HIS', 'Cài đặt PMS/LIS/HIS đã được sửa đổi bởi ###<admin_fullname>### (###<admin_id>###). Vui lòng đăng nhập lại để áp dụng các thay đổi.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='CS01' LIMIT 1), 
'hi', 'पीएमएस/एलआईएस/एचआईएस सेटिंग अपडेट', 'PMS/LIS/HIS सेटिंग को ###<admin_fullname>### (###<admin_id>###) द्वारा संशोधित किया गया है। कृपया परिवर्तन लागू करने के लिए पुनः लॉग इन करें।',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='CS01' LIMIT 1), 
'pt', 'Atualização de configuração PMS/LIS/HIS', 'A configuração PMS/LIS/HIS foi modificada por ###<admin_fullname>### (###<admin_id>###). Faça login novamente para aplicar as alterações.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='CS01' LIMIT 1), 
'ru', 'Обновление настроек PMS/LIS/HIS', 'Параметр PMS/LIS/HIS был изменен ###<admin_fullname>### (###<admin_id>###). Пожалуйста, войдите еще раз, чтобы применить изменения.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='CS01' LIMIT 1), 
'ja', 'PMS/LIS/HIS設定の更新', 'PMS/LIS/HIS 設定が ###<admin_fullname>### (###<admin_id>###) によって変更されました。変更を適用するには再度ログインしてください。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='CS01' LIMIT 1), 
'de', 'PMS/LIS/HIS-Einstellungsaktualisierung', 'Die PMS/LIS/HIS-Einstellung wurde von ###<admin_fullname>### (###<admin_id>###) geändert. Bitte melden Sie sich erneut an, um die Änderungen zu übernehmen.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='CS01' LIMIT 1), 
'id', 'Pembaruan Pengaturan PMS/LIS/NYA', 'Setting PMS/LIS/HIS telah diubah oleh ###<admin_fullname>### (###<admin_id>###). Silakan masuk lagi untuk menerapkan perubahan.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='CS01' LIMIT 1), 
'ko', 'PMS/LIS/HIS 설정 업데이트', 'PMS/LIS/HIS 설정이 ###<admin_fullname>###(###<admin_id>###)에 의해 수정되었습니다. 변경 사항을 적용하려면 다시 로그인하세요.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='CS01' LIMIT 1), 
'fr', 'Mise à jour des paramètres PMS/LIS/HIS', 'Le paramètre PMS/LIS/HIS a été modifié par ###<admin_fullname>### (###<admin_id>###). Veuillez vous reconnecter pour appliquer les modifications.',
NOW(), 'SYSTEM');

-- Successful Addition of New Device -------
INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS01' LIMIT 1), 
'en', 'Successful Addition of New Device', 'A new analyzer named ###<analyzer_name>### has been added by ###<admin_fullname>### (###<admin_id>###). Please log in again to apply the changes.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS01' LIMIT 1), 
'zh-Hans', '成功添加新设备', '###<admin_fullname>### (###<admin_id>###) 已添加名为 ###<analyzer_name>### 的新分析器。请重新登录以应用更改。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS01' LIMIT 1), 
'zh-Hant', '成功新增設備', '###<admin_fullname>### (###<admin_id>###) 新增了一個名為 ###<analyzer_name>### 的新分析器。請重新登入以套用變更。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS01' LIMIT 1), 
'es', 'Adición exitosa de un nuevo dispositivo', '###<admin_fullname>### (###<admin_id>###) agregó un nuevo analizador llamado ###<analyzer_name>###. Inicie sesión nuevamente para aplicar los cambios.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS01' LIMIT 1), 
'vi', 'Bổ sung thành công thiết bị mới', 'Một máy phân tích mới có tên ###<analyzer_name>### đã được thêm bởi ###<admin_fullname>### (###<admin_id>###). Vui lòng đăng nhập lại để áp dụng các thay đổi.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS01' LIMIT 1), 
'hi', 'नए डिवाइस का सफलतापूर्वक जोड़', '###<analyzer_name>### नामक एक नया विश्लेषक ###<admin_fullname>### (###<admin_id>###) द्वारा जोड़ा गया है। कृपया परिवर्तन लागू करने के लिए फिर से लॉग इन करें।',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS01' LIMIT 1), 
'pt', 'Adição bem-sucedida de novo dispositivo', 'Um novo analisador chamado ###<analyzer_name>### foi adicionado por ###<admin_fullname>### (###<admin_id>###). Faça login novamente para aplicar as alterações.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS01' LIMIT 1), 
'ru', 'Успешное добавление нового устройства', 'Новый анализатор с именем ###<analyzer_name>### был добавлен пользователем ###<admin_fullname>### (###<admin_id>###). Пожалуйста, войдите еще раз, чтобы применить изменения.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS01' LIMIT 1), 
'ja', '新しいデバイスの追加に成功しました', '###<analyzer_name>### という名前の新しいアナライザーが ###<admin_fullname>### (###<admin_id>###) によって追加されました。変更を適用するには、再度ログインしてください。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS01' LIMIT 1), 
'de', 'Erfolgreiches Hinzufügen eines neuen Geräts', 'Ein neuer Analysator mit dem Namen ###<analyzer_name>### wurde von ###<admin_fullname>### (###<admin_id>###) hinzugefügt. Bitte melden Sie sich erneut an, um die Änderungen zu übernehmen.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS01' LIMIT 1), 
'id', 'Penambahan Perangkat Baru Berhasil', 'Penganalisis baru bernama ###<analyzer_name>### telah ditambahkan oleh ###<admin_fullname>### (###<admin_id>###). Silakan masuk lagi untuk menerapkan perubahan.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS01' LIMIT 1), 
'ko', '새로운 장치의 성공적인 추가', '###<admin_fullname>###(###<admin_id>###)에 의해 ###<analyzer_name>###이라는 새 분석기가 추가되었습니다. 변경 사항을 적용하려면 다시 로그인하세요.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS01' LIMIT 1), 
'fr', 'Ajout réussi d''un nouvel appareil', 'Un nouvel analyseur nommé ###<analyzer_name>### a été ajouté par ###<admin_fullname>### (###<admin_id>###). Veuillez vous reconnecter pour appliquer les modifications.',
NOW(), 'SYSTEM');

-- Device Details Change ----------
INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS02' LIMIT 1), 
'en', 'Device Details Change', 'The analyzer previously known as ###<analyzer_name>### has been renamed to ###<new_analyzer_name>### by ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS02' LIMIT 1), 
'zh-Hans', '设备详细信息变更', '先前称为 ###<analyzer_name>### 的分析器已被 ###<admin_fullname>### (###<admin_id>###) 重命名为 ###<new_analyzer_name>###。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS02' LIMIT 1), 
'zh-Hant', '設備詳細資訊更改', '先前稱為 ###<analyzer_name>### 的分析器已由 ###<admin_fullname>### (###<admin_id>###) 重新命名為 ###<new_analyzer_name>###。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS02' LIMIT 1), 
'es', 'Cambio de detalles del dispositivo', 'El analizador anteriormente conocido como ###<analyzer_name>### ha sido renombrado a ###<new_analyzer_name>### por ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS02' LIMIT 1), 
'vi', 'Thay đổi chi tiết thiết bị', 'Máy phân tích trước đây có tên là ###<analyzer_name>### đã được đổi tên thành ###<new_analyzer_name>### bởi ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS02' LIMIT 1), 
'hi', 'डिवाइस विवरण बदलें', 'विश्लेषक जिसे पहले ###<analyzer_name>### के नाम से जाना जाता था, का नाम बदलकर ###<admin_fullname>### (###<admin_id>###) द्वारा ###<new_analyzer_name>### कर दिया गया है।',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS02' LIMIT 1), 
'pt', 'Alteração de detalhes do dispositivo', 'O analisador anteriormente conhecido como ###<analyzer_name>### foi renomeado para ###<new_analyzer_name>### por ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS02' LIMIT 1), 
'ru', 'Изменение сведений об устройстве', 'Анализатор, ранее известный как ###<имя_анализатора>###, был переименован в ###<новое_имя_анализатора>### пользователем ###<полное_имя_администратора>### (###<id_admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS02' LIMIT 1), 
'ja', 'デバイスの詳細の変更', '以前は ###<analyzer_name>### と呼ばれていたアナライザーは、###<admin_fullname>### (###<admin_id>###) によって ###<new_analyzer_name>### に名前が変更されました。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS02' LIMIT 1), 
'de', 'Gerätedetails ändern', 'Der zuvor als ###<analyzer_name>### bekannte Analysator wurde von ###<admin_fullname>### (###<admin_id>###) in ###<new_analyzer_name>### umbenannt.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS02' LIMIT 1), 
'id', 'Perubahan Detail Perangkat', 'Penganalisis yang sebelumnya dikenal sebagai ###<analyzer_name>### telah diubah namanya menjadi ###<new_analyzer_name>### oleh ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS02' LIMIT 1), 
'ko', '기기 세부정보 변경', '이전에 ###<analyzer_name>###으로 알려진 분석기는 ###<admin_fullname>###(###<admin_id>###)에 의해 ###<new_analyzer_name>###으로 이름이 변경되었습니다.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS02' LIMIT 1), 
'fr', 'Modification des détails de l''appareil', 'L''analyseur précédemment connu sous le nom de ###<analyzer_name>### a été renommé ###<new_analyzer_name>### par ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');


-- Device Removal --------------------
INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS03' LIMIT 1), 
'en', 'Device Removal', 'The analyzer named ###<analyzer_name>### has been deleted by ###<admin_fullname>### (###<admin_id>###). Please log in again to apply the changes.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS03' LIMIT 1), 
'zh-Hans', '设备移除', '名为 ###<analyzer_name>### 的分析器已被 ###<admin_fullname>### (###<admin_id>###) 删除。请重新登录以应用更改。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS03' LIMIT 1), 
'zh-Hant', '裝置移除', '名為 ###<analyzer_name>### 的分析器已被 ###<admin_fullname>### (###<admin_id>###) 刪除。請重新登入以套用變更。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS03' LIMIT 1), 
'es', 'Eliminación del dispositivo', 'El analizador llamado ###<analyzer_name>### ha sido eliminado por ###<admin_fullname>### (###<admin_id>###). Inicie sesión nuevamente para aplicar los cambios.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS03' LIMIT 1), 
'vi', 'Xóa thiết bị', 'Máy phân tích có tên ###<analyzer_name>### đã bị xóa bởi ###<admin_fullname>### (###<admin_id>###). Vui lòng đăng nhập lại để áp dụng các thay đổi.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS03' LIMIT 1), 
'hi', 'डिवाइस हटाना', '###<analyzer_name>### नामक विश्लेषक को ###<admin_fullname>### (###<admin_id>###) द्वारा हटा दिया गया है। कृपया परिवर्तन लागू करने के लिए फिर से लॉग इन करें।',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS03' LIMIT 1), 
'pt', 'Remoção de dispositivo', 'O analisador denominado ###<analyzer_name>### foi excluído por ###<admin_fullname>### (###<admin_id>###). Faça login novamente para aplicar as alterações.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS03' LIMIT 1), 
'ru', 'Удаление устройства', 'Анализатор с именем ###<имя_анализатора>### был удален пользователем ###<полное_имя_администратора>### (###<id_admin_id>###). Пожалуйста, войдите еще раз, чтобы применить изменения.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS03' LIMIT 1), 
'ja', 'デバイスの削除', '###<analyzer_name>### という名前のアナライザーは、###<admin_fullname>### (###<admin_id>###) によって削除されました。変更を適用するには、再度ログインしてください。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS03' LIMIT 1), 
'de', 'Geräteentfernung', 'Der Analysator mit dem Namen ###<analyzer_name>### wurde von ###<admin_fullname>### (###<admin_id>###) gelöscht. Bitte melden Sie sich erneut an, um die Änderungen zu übernehmen.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS03' LIMIT 1), 
'id', 'Penghapusan Perangkat', 'Penganalisis bernama ###<analyzer_name>### telah dihapus oleh ###<admin_fullname>### (###<admin_id>###). Silakan masuk lagi untuk menerapkan perubahan.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS03' LIMIT 1), 
'ko', '장치 제거', '###<analyzer_name>###이라는 분석기가 ###<admin_fullname>###(###<admin_id>###)에 의해 삭제되었습니다. 변경 사항을 적용하려면 다시 로그인하세요.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='DS03' LIMIT 1), 
'fr', 'Suppression du périphérique', 'L''analyseur nommé ###<analyzer_name>### a été supprimé par ###<admin_fullname>### (###<admin_id>###). Veuillez vous reconnecter pour appliquer les modifications.',
NOW(), 'SYSTEM');

-- Profile Update -------------
INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US01' LIMIT 1), 
'en', 'Profile Update', 'The staff details of ###<staff_fullname>### (###<staff_id>###) has been updated by ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US01' LIMIT 1), 
'zh-Hans', '个人资料更新', '###<staff_fullname>### (###<staff_id>###) 的员工详细信息已由 ###<admin_fullname>### (###<admin_id>###) 更新。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US01' LIMIT 1), 
'zh-Hant', '個人資料更新', '###<staff_fullname>### (###<staff_id>###) 的員工詳細資料已由 ###<admin_fullname>### (###<admin_id>###) 更新。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US01' LIMIT 1), 
'es', 'Actualización de perfil', 'Los detalles del personal de ###<staff_fullname>### (###<staff_id>###) han sido actualizados por ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US01' LIMIT 1), 
'vi', 'Cập nhật hồ sơ', 'Thông tin nhân viên của ###<staff_fullname>### (###<staff_id>###) đã được cập nhật bởi ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US01' LIMIT 1), 
'hi', 'प्रोफ़ाइल अपडेट', '###<staff_fullname>### (###<staff_id>###) के स्टाफ विवरण को ###<admin_fullname>### (###<admin_id>###) द्वारा अद्यतन किया गया है।',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US01' LIMIT 1), 
'pt', 'Atualização de perfil', 'Os detalhes da equipe de ###<staff_fullname>### (###<staff_id>###) foram atualizados por ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US01' LIMIT 1), 
'ru', 'Обновление профиля', 'Сведения о персонале ###<staff_fullname>### (###<staff_id>###) обновлены пользователем ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US01' LIMIT 1), 
'ja', 'プロフィールの更新', '###<staff_fullname>### (###<staff_id>###) のスタッフ詳細が ###<admin_fullname>### (###<admin_id>###) によって更新されました。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US01' LIMIT 1), 
'de', 'Profilaktualisierung', 'Die Mitarbeiterdaten von ###<staff_fullname>### (###<staff_id>###) wurden von ###<admin_fullname>### (###<admin_id>###) aktualisiert.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US01' LIMIT 1), 
'id', 'Pembaruan Profil', 'Detail staf ###<staff_fullname>### (###<staff_id>###) telah diperbarui oleh ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US01' LIMIT 1), 
'ko', '프로필 업데이트', '###<staff_fullname>###(###<staff_id>###)의 직원 세부 정보가 ###<admin_fullname>###(###<admin_id>###)에 의해 업데이트되었습니다.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US01' LIMIT 1), 
'fr', 'Mise à jour du profil', 'Les détails du personnel de ###<staff_fullname>### (###<staff_id>###) ont été mis à jour par ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

-- Profile Update US02 -------------
INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US02' LIMIT 1), 
'en', 'Profile Update', 'Your staff details has been updated. Please login in again to apply the changes.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US02' LIMIT 1), 
'zh-Hans', '个人资料更新', '您的员工详细信息已更新。请重新登录以应用更改。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US02' LIMIT 1), 
'zh-Hant', '個人資料更新', '您的員工詳細資料已更新。請重新登入以套用變更。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US02' LIMIT 1), 
'es', 'Actualización de perfil', 'Los datos de su personal han sido actualizados. Inicie sesión nuevamente para aplicar los cambios.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US02' LIMIT 1), 
'vi', 'Cập nhật hồ sơ', 'Thông tin chi tiết về nhân viên của bạn đã được cập nhật. Vui lòng đăng nhập lại để áp dụng các thay đổi.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US02' LIMIT 1), 
'hi', 'प्रोफ़ाइल अपडेट', 'आपके स्टाफ का विवरण अपडेट कर दिया गया है। कृपया परिवर्तन लागू करने के लिए पुनः लॉगिन करें।',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US02' LIMIT 1), 
'pt', 'Atualização de perfil', 'Os detalhes da sua equipe foram atualizados. Faça login novamente para aplicar as alterações.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US02' LIMIT 1), 
'ru', 'Обновление профиля', 'Информация о вашем персонале обновлена. Пожалуйста, войдите в систему еще раз, чтобы применить изменения.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US02' LIMIT 1), 
'ja', 'プロフィールの更新', 'スタッフの詳細が更新されました。変更を適用するには再度ログインしてください。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US02' LIMIT 1), 
'de', 'Profilaktualisierung', 'Ihre Mitarbeiterdaten wurden aktualisiert. Bitte melden Sie sich erneut an, um die Änderungen zu übernehmen.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US02' LIMIT 1), 
'id', 'Pembaruan Profil', 'Detail staf Anda telah diperbarui. Silakan login kembali untuk menerapkan perubahan.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US02' LIMIT 1), 
'ko', '프로필 업데이트', '직원 세부 정보가 업데이트되었습니다. 변경 사항을 적용하려면 다시 로그인하세요.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US02' LIMIT 1), 
'fr', 'Mise à jour du profil', 'Les détails de votre personnel ont été mis à jour. Veuillez vous reconnecter pour appliquer les modifications.',
NOW(), 'SYSTEM');

-- Staff Reactivation  --------------
INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US03' LIMIT 1), 
'en', 'Staff Reactivation', 'The user account of ###<staff_fullname>### (###<staff_id>###) has been reactivated by ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US03' LIMIT 1), 
'zh-Hans', '员工重新启动', '###<staff_fullname>### (###<staff_id>###) 的用户帐户已被 ###<admin_fullname>### (###<admin_id>###) 重新激活。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US03' LIMIT 1), 
'zh-Hant', '員工重新啟動', '###<staff_fullname>### (###<staff_id>###) 的使用者帳號已被 ###<admin_fullname>### (###<admin_id>###) 重新啟動。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US03' LIMIT 1), 
'es', 'Reactivación del personal', 'La cuenta de usuario de ###<staff_fullname>### (###<staff_id>###) ha sido reactivada por ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US03' LIMIT 1), 
'vi', 'Kích hoạt lại nhân viên', 'Tài khoản người dùng của ###<staff_fullname>### (###<staff_id>###) đã được kích hoạt lại bởi ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US03' LIMIT 1), 
'hi', 'स्टाफ पुनः सक्रियण', '###<staff_fullname>### (###<staff_id>###) का उपयोगकर्ता खाता ###<admin_fullname>### (###<admin_id>###) द्वारा पुनः सक्रिय कर दिया गया है।',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US03' LIMIT 1), 
'pt', 'Reativação de Pessoal', 'A conta de usuário ###<staff_fullname>### (###<staff_id>###) foi reativada por ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US03' LIMIT 1), 
'ru', 'Реактивация персонала', 'Учетная запись пользователя ###<staff_fullname>### (###<staff_id>###) была повторно активирована пользователем ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US03' LIMIT 1), 
'ja', 'スタッフの再活性化', '###<staff_fullname>### (###<staff_id>###) のユーザー アカウントが、###<admin_fullname>### (###<admin_id>###) によって再アクティブ化されました。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US03' LIMIT 1), 
'de', 'Reaktivierung des Personals', 'Das Benutzerkonto von ###<staff_fullname>### (###<staff_id>###) wurde von ###<admin_fullname>### (###<admin_id>###) reaktiviert.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US03' LIMIT 1), 
'id', 'Pengaktifan Kembali Staf', 'Akun pengguna ###<staff_fullname>### (###<staff_id>###) telah diaktifkan kembali oleh ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US03' LIMIT 1), 
'ko', '직원 재활성화', '###<staff_fullname>###(###<staff_id>###)의 사용자 계정이 ###<admin_fullname>###(###<admin_id>###)에 의해 다시 활성화되었습니다.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US03' LIMIT 1), 
'fr', 'Réactivation du personnel', 'Le compte utilisateur de ###<staff_fullname>### (###<staff_id>###) a été réactivé par ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

-- Staff Deactivation --------------
INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US04' LIMIT 1), 
'en', 'Staff Deactivation', 'The user account of ###<staff_fullname>### (###<staff_id>###) has been deactivated by ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US04' LIMIT 1), 
'zh-Hans', '员工停用', '###<staff_fullname>### (###<staff_id>###) 的用户帐户已被 ###<admin_fullname>### (###<admin_id>###) 停用。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US04' LIMIT 1), 
'zh-Hant', '員工停用', '###<staff_fullname>### (###<staff_id>###) 的使用者帳號已停用 ###<admin_fullname>### (###<admin_id>###) 停用。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US04' LIMIT 1), 
'es', 'Desactivación de personal', 'La cuenta de usuario de ###<staff_fullname>### (###<staff_id>###) ha sido desactivada por ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US04' LIMIT 1), 
'vi', 'Vô hiệu hóa nhân viên', 'Tài khoản người dùng của ###<staff_fullname>### (###<staff_id>###) đã bị vô hiệu hóa bởi ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US04' LIMIT 1), 
'hi', 'स्टाफ निष्क्रियण', '###<staff_fullname>### (###<staff_id>###) का उपयोगकर्ता खाता ###<admin_fullname>### (###<admin_id>###) द्वारा निष्क्रिय कर दिया गया है।',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US04' LIMIT 1), 
'pt', 'Desativação de Pessoal', 'A conta de usuário ###<staff_fullname>### (###<staff_id>###) foi desativada por ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US04' LIMIT 1), 
'ru', 'Деактивация персонала', 'Учетная запись пользователя ###<staff_fullname>### (###<staff_id>###) деактивирована пользователем ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US04' LIMIT 1), 
'ja', 'スタッフの非アクティブ化', '###<staff_fullname>### (###<staff_id>###) のユーザー アカウントは、###<admin_fullname>### (###<admin_id>###) によって非アクティブ化されました。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US04' LIMIT 1), 
'de', 'Personaldeaktivierung', 'Das Benutzerkonto von ###<staff_fullname>### (###<staff_id>###) wurde von ###<admin_fullname>### (###<admin_id>###) deaktiviert.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US04' LIMIT 1), 
'id', 'Penonaktifan Staf', 'Akun pengguna ###<staff_fullname>### (###<staff_id>###) telah dinonaktifkan oleh ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US04' LIMIT 1), 
'ko', '직원 비활성화', '###<staff_fullname>###(###<staff_id>###)의 사용자 계정이 ###<admin_fullname>###(###<admin_id>###)에 의해 비활성화되었습니다.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US04' LIMIT 1), 
'fr', 'Désactivation du personnel', 'Le compte utilisateur de ###<staff_fullname>### (###<staff_id>###) a été désactivé par ###<admin_fullname>### (###<admin_id>###).',
NOW(), 'SYSTEM');

-- Successful Creation of New User -------
INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US05' LIMIT 1), 
'en', 'Successful Creation of New User', 'The successful creation of a new user with Staff ID: ###<staff_id>###, Staff Name: ###<staff_fullname>###.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US05' LIMIT 1), 
'zh-Hans', '成功创建新用户', '成功创建一个新用户，员工 ID：###<staff_id>###，员工姓名：###<staff_fullname>###。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US05' LIMIT 1), 
'zh-Hant', '新用戶創建成功', '已成功建立新用戶，員工 ID：###<staff_id>###，員工姓名：###<staff_fullname>###。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US05' LIMIT 1), 
'es', 'Creación exitosa de nueva usuaria', 'La creación exitosa de un nuevo usuario con ID de personal: ###<staff_id>###, Nombre del personal: ###<staff_fullname>###.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US05' LIMIT 1), 
'vi', 'Tạo thành công người dùng mới', 'Tạo thành công người dùng mới với ID nhân viên: ###<staff_id>###, Tên nhân viên: ###<staff_fullname>###.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US05' LIMIT 1), 
'hi', 'नए उपयोगकर्ता का सफल निर्माण', 'स्टाफ आईडी के साथ एक नए उपयोगकर्ता का सफलतापूर्वक निर्माण: ###<staff_id>###, स्टाफ नाम: ###<staff_fullname>###.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US05' LIMIT 1), 
'pt', 'Criação bem-sucedida de novo usuário', 'A criação bem-sucedida de um novo usuário com ID da equipe: ###<staff_id>###, Nome da equipe: ###<staff_fullname>###.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US05' LIMIT 1), 
'ru', 'Успешное создание нового пользователя', 'Успешное создание нового пользователя с идентификатором персонала: ###<staff_id>###, именем сотрудника: ###<staff_fullname>###.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US05' LIMIT 1), 
'ja', '新規ユーザーの作成に成功しました', 'スタッフ ID: ###<staff_id>###、スタッフ名: ###<staff_fullname>### の新しいユーザーが正常に作成されました。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US05' LIMIT 1), 
'de', 'Erfolgreiches Anlegen eines neuen Benutzers', 'Die erfolgreiche Erstellung eines neuen Benutzers mit Mitarbeiter-ID: ###<staff_id>###, Mitarbeitername: ###<staff_fullname>###.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US05' LIMIT 1), 
'id', 'Keberhasilan Pembuatan Pengguna Baru', 'Berhasilnya pembuatan pengguna baru dengan ID Staf: ###<staff_id>###, Nama Staf: ###<staff_fullname>###.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US05' LIMIT 1), 
'ko', '신규 사용자 생성 성공', '직원 ID: ###<staff_id>###, 직원 이름: ###<staff_fullname>###을 사용하여 새 사용자가 성공적으로 생성되었습니다.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='US05' LIMIT 1), 
'fr', 'Création réussie d''un nouvel utilisateur', 'La création réussie d''un nouvel utilisateur avec l''ID du personnel : ###<staff_id>###, le nom du personnel : ###<staff_fullname>###.',
NOW(), 'SYSTEM');


-- New Account Creation ---------------
INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN01' LIMIT 1), 
'en', 'New Account Creation', 
'<!DOCTYPE html>
<html>
	<body>
		Dear ###<staff_fullname>###, </br></br>

		We are pleased to inform you that your account has been successfully created!</br></br>

		Below are your login details:</br>
		Login ID: ###<login_id>###</br>
		Temporary Password: ###<password>###</br></br>

		Please refrain from replying to this email as it is auto-generated.</br></br>

		Best regards,</br>
		VCheck Viewer Team</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN01' LIMIT 1), 
'zh-Hans', '创建新帐户', 
'<!DOCTYPE html>
<html>
	<body>
	亲爱的 ###<staff_fullname>###，</br></br>

	我们很高兴地通知您，您的帐户已成功创建！</br></br>

	以下是您的登录详细信息：</br>
	登录 ID：###<login_id>###</br>
	临时密码：###<password>###</br></br>

	请不要回复此电子邮件，因为它是自动生成的。</br></br>

	此致，</br>
	VCheck Viewer 团队</br>
	</body>
</html>',
NOW(), 'SYSTEM');


INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN01' LIMIT 1), 
'zh-Hant', '新帳戶創建', 
'<!DOCTYPE html>
<html>
	<body>
	亲爱的 ###<staff_fullname>###，</br></br>

	我们很高兴地通知您，您的帐户已成功创建！</br></br>

	以下是您的登录详细信息：</br>
	登录 ID：###<login_id>###</br>
	临时密码：###<password>###</br></br>

	请不要回复此电子邮件，因为它是自动生成的。</br></br>

	此致，</br>
	VCheck Viewer 团队</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN01' LIMIT 1), 
'es', 'Creación de nueva cuenta', 
'<!DOCTYPE html>
<html>
	<body>
	  Estimado ###<staff_fullname>###, </br></br>

	  ¡Nos complace informarle que su cuenta se ha creado correctamente!</br></br>

	  A continuación se muestran sus datos de inicio de sesión:</br>
	  ID de inicio de sesión: ###<login_id>###</br>
	  Contraseña temporal: ###<password>###</br></br>

	  Absténgase de responder a este correo electrónico ya que se genera automáticamente.</br></br>

	  Saludos cordiales,</br>
	  Equipo de visores de VCheck</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN01' LIMIT 1), 
'vi', 'Tạo tài khoản mới', 
'<!DOCTYPE html>
<html>
	<body>
	  Kính gửi ###<staff_fullname>###, </br></br>

	  Chúng tôi vui mừng thông báo với bạn rằng tài khoản của bạn đã được tạo thành công!</br></br>

	  Dưới đây là chi tiết đăng nhập của bạn:</br>
	  ID đăng nhập: ###<login_id>###</br>
	  Mật khẩu tạm thời: ###<password>###</br></br>

	  Vui lòng không trả lời email này vì nó được tạo tự động.</br></br>

	  Trân trọng,</br>
	  Nhóm người xem VCheck</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN01' LIMIT 1), 
'hi', 'नया खाता निर्माण', 
'<!DOCTYPE html>
<html>
	<body>
		प्रिय ###<staff_fullname>###, </br></br>

		हमें आपको यह बताते हुए खुशी हो रही है कि आपका खाता सफलतापूर्वक बना लिया गया है!</br></br>

		नीचे आपके लॉगिन विवरण दिए गए हैं:</br>
		लॉगिन आईडी: ###<login_id>###</br>
		अस्थायी पासवर्ड: ###<password>###</br></br>

		कृपया इस ईमेल का उत्तर देने से बचें क्योंकि यह स्वतः जनरेट किया गया है।</br></br>

		सादर,</br>
		VCheck व्यूअर टीम</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN01' LIMIT 1), 
'pt', 'Criação de nova conta', 
'<!DOCTYPE html>
<html>
	<body>
	Prezado ###<staff_fullname>###, </br></br>

	  Temos o prazer de informar que sua conta foi criada com sucesso!</br></br>

	  Abaixo estão seus dados de login:</br>
	  ID de login: ###<login_id>###</br>
	  Senha temporária: ###<password>###</br></br>

	  Evite responder a este e-mail, pois ele é gerado automaticamente.</br></br>

	  Atenciosamente,</br>
	  Equipe do visualizador VCheck</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN01' LIMIT 1), 
'ru', 'Создание новой учетной записи', 
'<!DOCTYPE html>
<html>
	<body>
		Дорогой ###<staff_fullname>###, </br></br>

		  Мы рады сообщить Вам, что Ваша учетная запись успешно создана!</br></br>

		  Ниже приведены ваши данные для входа:</br>
		  Идентификатор входа: ###<login_id>###</br>
		  Временный пароль: ###<password>###</br></br>

		  Пожалуйста, воздержитесь от ответа на это письмо, поскольку оно создано автоматически.</br></br>

		  С уважением,</br>
		  Команда просмотра VCheck</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN01' LIMIT 1), 
'ja', '新規アカウントの作成', 
'<!DOCTYPE html>
<html>
	<body>
		###<staff_fullname>### 様</br></br>

		アカウントが正常に作成されたことをお知らせいたします。</br></br>

		ログイン情報は以下の通りです:</br>
		ログイン ID: ###<login_id>###</br>
		仮パスワード: ###<password>###</br></br>

		このメールは自動生成されるため、返信しないでください。</br></br>

		よろしくお願いいたします。</br>

		VCheck Viewer チーム</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN01' LIMIT 1), 
'de', 'Neues Konto erstellen', 
'<!DOCTYPE html>
<html>
	<body>
		Sehr geehrte(r) ###<staff_fullname>###, </br></br>

		Wir freuen uns, Ihnen mitteilen zu können, dass Ihr Konto erfolgreich erstellt wurde!</br></br>

		Nachfolgend finden Sie Ihre Anmeldedaten:</br>
		Anmelde-ID: ###<login_id>###</br>
		Temporäres Passwort: ###<password>###</br></br>

		Bitte antworten Sie nicht auf diese E-Mail, da sie automatisch generiert wird.</br></br>

		Mit freundlichen Grüßen,</br>
		VCheck Viewer Team</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN01' LIMIT 1), 
'id', 'Pembuatan Akun Baru', 
'<!DOCTYPE html>
<html>
	<body>
	  Yang terhormat ###<staff_fullname>###, </br></br>

	  Dengan senang hati kami informasikan bahwa akun Anda telah berhasil dibuat!</br></br>

	  Di bawah ini adalah detail login Anda:</br>
	  ID Masuk: ###<login_id>###</br>
	  Kata Sandi Sementara: ###<password>###</br></br>

	  Harap jangan membalas email ini karena email ini dibuat secara otomatis.</br></br>

	  Hormat kami,</br>
	  Tim Penampil VCheck</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN01' LIMIT 1), 
'ko', '새 계정 생성', 
'<!DOCTYPE html>
<html>
	<body>
	  ###<staff_fullname>###님, </br></br>님께

	  귀하의 계정이 성공적으로 생성되었음을 알려드리게 되어 기쁘게 생각합니다!</br></br>

	  귀하의 로그인 정보는 다음과 같습니다:</br>
	  로그인 ID: ###<login_id>###</br>
	  임시 비밀번호: ###<password>###</br></br>

	  본 이메일은 자동으로 생성된 이메일이므로 회신을 삼가해 주시기 바랍니다.</br></br>

	  감사합니다.</br>
	  VCheck 뷰어 팀</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN01' LIMIT 1), 
'fr', 'Création d''un nouveau compte', 
'<!DOCTYPE html>
<html>
	<body>
	  Cher ###<staff_fullname>###, </br></br>

	  Nous sommes heureux de vous informer que votre compte a été créé avec succès !</br></br>

	  Vous trouverez ci-dessous vos informations de connexion :</br>
	  Identifiant de connexion : ###<login_id>###</br>
	  Mot de passe temporaire : ###<password>###</br></br>

	  Veuillez vous abstenir de répondre à cet e-mail car il est généré automatiquement.</br></br>

	  Cordialement,</br>
	  Équipe de visualisation VCheck</br>
	</body>
</html>',
NOW(), 'SYSTEM');


-- Password Reset & Recovery ----------------
INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN02' LIMIT 1), 
'en', 'Password Reset & Recovery', 
'<!DOCTYPE html>
<html>
	<body>
		Dear ###<staff_fullname>###, </br></br>

		This is your temporary password ###<password>###.</br></br>

		Please utilize this temporary password to regain access to your account.</br></br>
		
		Please refrain from replying to this email as it is auto-generated.</br></br>

		Best regards,</br>
		VCheck Viewer Team</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN02' LIMIT 1), 
'zh-Hans', '密码重置和恢复', 
'<!DOCTYPE html>
<html>
	<body>
		亲爱的 ###<staff_fullname>###，</br></br>

		这是您的临时密码 ###<password>###。</br></br>

		请使用此临时密码重新访问您的帐户。</br></br>

		请不要回复此电子邮件，因为它是自动生成的。</br></br>

		此致，</br>
		VCheck Viewer 团队</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN02' LIMIT 1), 
'zh-Hant', '密碼重置和恢復', 
'<!DOCTYPE html>
<html>
	<body>
		  親愛的###<staff_fullname>###，</br></br>

		  這是您的臨時密碼###<password>###。</br></br>

		  請使用此臨時密碼重新造訪您的帳戶。</br></br>

		  請不要回覆此電子郵件，因為它是自動產生的。</br></br>

		  謹致問候，</br>
		  VCheck檢視器團隊</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN02' LIMIT 1), 
'es', 'Restablecimiento y recuperación de contraseña', 
'<!DOCTYPE html>
<html>
	<body>
	  Estimado ###<staff_fullname>###, </br></br>

	  Esta es su contraseña temporal ###<password>###.</br></br>

	  Utilice esta contraseña temporal para recuperar el acceso a su cuenta.</br></br>

	  Absténgase de responder a este correo electrónico ya que se genera automáticamente.</br></br>

	  Saludos cordiales,</br>
	  Equipo de visores de VCheck</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN02' LIMIT 1), 
'vi', 'Đặt lại và khôi phục mật khẩu', 
'<!DOCTYPE html>
<html>
	<body>
	  Kính gửi ###<staff_fullname>###, </br></br>

	  Đây là mật khẩu tạm thời của bạn ###<password>###.</br></br>

	  Vui lòng sử dụng mật khẩu tạm thời này để lấy lại quyền truy cập vào tài khoản của bạn.</br></br>

	  Vui lòng không trả lời email này vì nó được tạo tự động.</br></br>

	  Trân trọng,</br>
	  Nhóm người xem VCheck</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN02' LIMIT 1), 
'hi', 'पासवर्ड रीसेट और पुनर्प्राप्ति', 
'<!DOCTYPE html>
<html>
	<body>
		प्रिय ###<staff_fullname>###, </br></br>

		यह आपका अस्थायी पासवर्ड है ###<password>###.</br></br>

		कृपया अपने खाते तक पहुँच पुनः प्राप्त करने के लिए इस अस्थायी पासवर्ड का उपयोग करें.</br></br>

		कृपया इस ईमेल का उत्तर देने से बचें क्योंकि यह स्वतः जनरेट किया गया है.</br></br>

		सादर,</br>
		VCheck व्यूअर टीम</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN02' LIMIT 1), 
'pt', 'Redefinição e recuperação de senha', 
'<!DOCTYPE html>
<html>
	<body>
		  Prezado ###<staff_fullname>###, </br></br>

		  Esta é sua senha temporária ###<password>###.</br></br>

		  Utilize esta senha temporária para recuperar o acesso à sua conta.</br></br>

		  Evite responder a este e-mail, pois ele é gerado automaticamente.</br></br>

		  Atenciosamente,</br>
		  Equipe do visualizador VCheck</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN02' LIMIT 1), 
'ru', 'Сброс и восстановление пароля', 
'<!DOCTYPE html>
<html>
	<body>
	  Дорогой ###<staff_fullname>###, </br></br>

	  Это ваш временный пароль ###<password>###.</br></br>

	  Используйте этот временный пароль, чтобы восстановить доступ к своей учетной записи.</br></br>

	  Пожалуйста, воздержитесь от ответа на это письмо, поскольку оно создано автоматически.</br></br>

	  С уважением,</br>
	  Команда просмотра VCheck</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN02' LIMIT 1), 
'ja', 'パスワードのリセットと回復', 
'<!DOCTYPE html>
<html>
	<body>
		###<staff_fullname>### 様、</br></br>

		これはあなたの仮パスワード ###<password>### です。</br></br>

		この仮パスワードを使用して、アカウントに再度アクセスしてください。</br></br>

		このメールは自動生成されるため、返信しないでください。</br></br>

		よろしくお願いいたします。</br>

		VCheck Viewer チーム</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN02' LIMIT 1), 
'de', 'Zurücksetzen und Wiederherstellen des Passworts', 
'<!DOCTYPE html>
<html>
	<body>
		Sehr geehrte/r ###<staff_fullname>###, </br></br>

		Dies ist Ihr temporäres Passwort ###<password>###.</br></br>

		Bitte verwenden Sie dieses temporäre Passwort, um wieder Zugriff auf Ihr Konto zu erhalten.</br></br>

		Bitte antworten Sie nicht auf diese E-Mail, da sie automatisch generiert wird.</br></br>

		Mit freundlichen Grüßen,</br>
		VCheck Viewer Team</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN02' LIMIT 1), 
'id', 'Reset & Pemulihan Kata Sandi', 
'<!DOCTYPE html>
<html>
	<body>
	  Yang terhormat ###<staff_fullname>###, </br></br>

	  Ini adalah sandi sementara Anda ###<password>###.</br></br>

	  Harap gunakan sandi sementara ini untuk mendapatkan kembali akses ke akun Anda.</br></br>

	  Harap jangan membalas email ini karena email ini dibuat secara otomatis.</br></br>

	  Hormat kami,</br>
	  Tim Penampil VCheck</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN02' LIMIT 1), 
'ko', '비밀번호 재설정 및 복구', 
'<!DOCTYPE html>
<html>
	<body>
	  ###<staff_fullname>###님, </br></br>님께

	  임시 비밀번호 ###<password>###입니다.</br></br>

	  귀하의 계정에 다시 액세스하려면 이 임시 비밀번호를 활용하십시오.</br></br>

	  본 이메일은 자동으로 생성된 이메일이므로 회신을 삼가해 주시기 바랍니다.</br></br>

	  감사합니다.</br>
	  VCheck 뷰어 팀</br>
	</body>
</html>',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='EN02' LIMIT 1), 
'fr', 'Réinitialisation et récupération du mot de passe', 
'<!DOCTYPE html>
<html>
	<body>
	  Cher ###<staff_fullname>###, </br></br>

	  Ceci est votre mot de passe temporaire ###<password>###.</br></br>

	  Veuillez utiliser ce mot de passe temporaire pour retrouver l''accès à votre compte.</br></br>

	  Veuillez vous abstenir de répondre à cet e-mail car il est généré automatiquement.</br></br>

	  Cordialement,</br>
	  Équipe de visualisation VCheck</br>
	</body>
</html>',
NOW(), 'SYSTEM');

-- Reminder for Software Update -------------
INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='SF01' LIMIT 1), 
'en', 'Reminder for Software Update', 'Kindly remember to update your analyzer''s software/firmware for optimal performance. Visit ###<link>### to verify available updates.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='SF01' LIMIT 1), 
'zh-Hans', '软件更新提醒', 
'请记住更新分析仪的软件/固件以获得最佳性能。请访问 ###<link>### 以验证可用的更新。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='SF01' LIMIT 1), 
'zh-Hant', '軟體更新提醒', 
'請記住更新分析儀的軟體/韌體以獲得最佳效能。造訪 ###<link>### 以驗證可用更新。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='SF01' LIMIT 1), 
'es', 'Recordatorio de actualización de software', 
'Recuerde actualizar el software/firmware de su analizador para obtener un rendimiento óptimo. Visite ###<link>### para verificar las actualizaciones disponibles.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='SF01' LIMIT 1), 
'vi', 'Lời nhắc cập nhật phần mềm', 
'Vui lòng nhớ cập nhật phần mềm/chương trình cơ sở của máy phân tích để có hiệu suất tối ưu. Truy cập ###<link>### để xác minh các bản cập nhật có sẵn.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='SF01' LIMIT 1), 
'hi', 'सॉफ़्टवेयर अद्यतन के लिए अनुस्मारक', 
'कृपया अपने एनालाइजर के सॉफ्टवेयर/फर्मवेयर को बेहतर प्रदर्शन के लिए अपडेट करना न भूलें। उपलब्ध अपडेट की पुष्टि के लिए ###<link>### पर जाएं।',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='SF01' LIMIT 1), 
'pt', 'Lembrete para atualização de software', 
'Lembre-se de atualizar o software/firmware do seu analisador para obter o desempenho ideal. Visite ###<link>### para verificar as atualizações disponíveis.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='SF01' LIMIT 1), 
'ru', 'Напоминание об обновлении программного обеспечения', 
'Не забудьте обновить программное обеспечение/прошивку вашего анализатора для обеспечения оптимальной производительности. Посетите ###<link>###, чтобы проверить доступные обновления.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='SF01' LIMIT 1), 
'ja', 'ソフトウェアアップデートのリマインダー', 
'最適なパフォーマンスを得るには、アナライザーのソフトウェア/ファームウェアを必ず更新してください。利用可能なアップデートを確認するには、###<link>### にアクセスしてください。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='SF01' LIMIT 1), 
'de', 'Erinnerung zum Software-Update', 
'Denken Sie daran, die Software/Firmware Ihres Analysators zu aktualisieren, um eine optimale Leistung zu erzielen. Besuchen Sie ###<link>###, um zu prüfen, ob Updates verfügbar sind.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='SF01' LIMIT 1), 
'id', 'Pengingat untuk Pembaruan Perangkat Lunak', 
'Mohon diingat untuk memperbarui perangkat lunak/firmware penganalisis Anda untuk kinerja optimal. Kunjungi ###<link>### untuk memverifikasi pembaruan yang tersedia.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='SF01' LIMIT 1), 
'ko', '소프트웨어 업데이트 알림', 
'최적의 성능을 위해 분석기의 소프트웨어/펌웨어를 업데이트하는 것을 잊지 마십시오. 사용 가능한 업데이트를 확인하려면 ###<link>###를 방문하세요.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='SF01' LIMIT 1), 
'fr', 'Rappel pour la mise à jour du logiciel', 
'N''oubliez pas de mettre à jour le logiciel/micrologiciel de votre analyseur pour des performances optimales. Visitez ###<link>### pour vérifier les mises à jour disponibles.',
NOW(), 'SYSTEM');

----------- The Test Result is Available for Viewing -----------------
INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR01' LIMIT 1), 
'en', 'The Test Result is Available for Viewing', 'The test results for Patient ID: ###<patient_id>### are now accessible. Kindly navigate to the Results section to view or print the detailed report.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR01' LIMIT 1), 
'zh-Hans', '测试结果已可查看', '现在可以查看患者 ID：###<patient_id>### 的测试结果。请导航至结果部分以查看或打印详细报告。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR01' LIMIT 1), 
'zh-Hant', '測試結果可供查看', '病患 ID 的測試結果：###<patient_id>### 現已可存取。請導航至「結果」部分以查看或列印詳細報告。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR01' LIMIT 1), 
'es', 'El resultado de la prueba está disponible para su visualización', 'Ahora se puede acceder a los resultados de la prueba para ID de paciente: ###<patient_id>###. Navegue por favor a la sección Resultados para ver o imprimir el informe detallado.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR01' LIMIT 1), 
'vi', 'Kết quả kiểm tra có sẵn để xem', 'Hiện có thể truy cập kết quả xét nghiệm ID bệnh nhân: ###< Patient_id>###. Vui lòng điều hướng đến phần Kết quả để xem hoặc in báo cáo chi tiết.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR01' LIMIT 1), 
'hi', 'परीक्षा परिणाम देखने के लिए उपलब्ध है', 'रोगी आईडी: ###<patient_id>### के लिए परीक्षण परिणाम अब सुलभ हैं। विस्तृत रिपोर्ट देखने या प्रिंट करने के लिए कृपया परिणाम अनुभाग पर जाएँ।',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR01' LIMIT 1), 
'pt', 'O resultado do teste está disponível para visualização', 'Os resultados do teste para ID do paciente: ###<paciente_id>### agora estão acessíveis. Navegue até a seção Resultados para visualizar ou imprimir o relatório detalhado.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR01' LIMIT 1), 
'ru', 'Результат теста доступен для просмотра', 'Результаты теста для идентификатора пациента: ###<пациент_id>### теперь доступны. Пожалуйста, перейдите в раздел «Результаты», чтобы просмотреть или распечатать подробный отчет.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR01' LIMIT 1), 
'ja', 'テスト結果は閲覧可能です', '患者 ID: ###<patient_id>### のテスト結果にアクセスできるようになりました。詳細レポートを表示または印刷するには、結果セクションに移動してください。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR01' LIMIT 1), 
'de', 'Das Testergebnis steht zur Ansicht bereit', 'Die Testergebnisse für die Patienten-ID: ###<patient_id>### sind jetzt zugänglich. Navigieren Sie bitte zum Abschnitt „Ergebnisse“, um den ausführlichen Bericht anzuzeigen oder auszudrucken.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR01' LIMIT 1), 
'id', 'Hasil Tes Tersedia untuk Dilihat', 'Hasil tes ID Pasien: ###<patient_id>### kini dapat diakses. Silakan navigasikan ke bagian Hasil untuk melihat atau mencetak laporan rinci.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR01' LIMIT 1), 
'ko', '테스트 결과를 볼 수 있습니다', '이제 환자 ID: ###<patient_id>###에 대한 테스트 결과에 액세스할 수 있습니다. 세부 보고서를 보거나 인쇄하려면 결과 섹션으로 이동하십시오.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR01' LIMIT 1), 
'fr', 'Le résultat du test est disponible pour visualisation', 'Les résultats des tests pour l''ID patient : ###<patient_id>### sont désormais accessibles. Veuillez accéder à la section Résultats pour afficher ou imprimer le rapport détaillé.',
NOW(), 'SYSTEM');

-- Scheduled Test Appointment -----------------
INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR02' LIMIT 1), 
'en', 'Scheduled Test Appointment', 'A scheduled test is upcoming for Patient ID: ###<patient_id>###, Patient Name: ###<patient_name>###, set for ###<time>###. The attending doctor is ###<doctor_name>###.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR02' LIMIT 1), 
'zh-Hans', '预约测试', '即将对患者 ID：###<patient_id>###、患者姓名：###<patient_name>### 进行预约检查，时间为 ###<time>###。主治医生是 ###<doctor_name>###。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR02' LIMIT 1), 
'zh-Hant', '預約測試', '即將進行預定測試，患者 ID：###<patient_id>###，患者姓名：###<patient_name>###，設定為 ###<time>###。主治醫生是 ###<doctor_name>###。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR02' LIMIT 1), 
'es', 'Cita de prueba programada', 'Próximamente se realizará una prueba programada para ID del paciente: ###<patient_id>###, Nombre del paciente: ###<patient_name>###, configurado para ###<time>###. El médico tratante es ###<doctor_name>###.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR02' LIMIT 1), 
'vi', 'Lịch hẹn kiểm tra theo lịch trình', 'Sắp có một cuộc kiểm tra theo lịch cho ID Bệnh nhân: ###< Patient_id>###, Tên Bệnh nhân: ###<tên_bệnh nhân>###, được đặt cho ###<time>###. Bác sĩ điều trị là ###<doctor_name>###.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR02' LIMIT 1), 
'hi', 'अनुसूचित परीक्षण अपॉइंटमेंट', 'रोगी आईडी: ###<रोगी_आईडी>###, रोगी का नाम: ###<रोगी_नाम>###, ###<समय>### के लिए निर्धारित एक निर्धारित परीक्षण आने वाला है। उपस्थित चिकित्सक ###<डॉक्टर_नाम>### है।',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR02' LIMIT 1), 
'pt', 'Consulta de teste agendada', 'Um teste agendado está próximo para ID do paciente: ###<ident_paciente>###, Nome do paciente: ###<nome_paciente>###, definido para ###<hora>###. O médico responsável é ###<doctor_name>###.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR02' LIMIT 1), 
'ru', 'Запланированное тестовое назначение', 'Предстоит запланированное тестирование для идентификатора пациента: ###<идентификатор_пациента>###, имя пациента: ###<имя_пациента>###, установлено на ###<время>###. Лечащий врач: ###<имя_доктора>###.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR02' LIMIT 1), 
'ja', '予定されたテストの予約', '患者 ID: ###<patient_id>###、患者名: ###<patient_name>### の定期検査が ###<time>### に予定されています。担当医は ###<doctor_name>### です。',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR02' LIMIT 1), 
'de', 'Geplanter Testtermin', 'Ein geplanter Test steht bevor für Patienten-ID: ###<patient_id>###, Patientenname: ###<patient_name>###, festgelegt für ###<time>###. Der behandelnde Arzt ist ###<doctor_name>###.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR02' LIMIT 1), 
'id', 'Janji Tes Terjadwal', 'Tes terjadwal akan datang untuk ID Pasien: ###<patient_id>###, Nama Pasien: ###<patient_name>###, ditetapkan untuk ###<time>###. Dokter yang merawat adalah ###<nama_dokter>###.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR02' LIMIT 1), 
'ko', '예정된 테스트 예약', '환자 ID: ###<patient_id>###, 환자 이름: ###<patient_name>###, ###<time>###에 대해 설정된 테스트가 예정되어 있습니다. 주치의는 ###<doctor_name>###입니다.',
NOW(), 'SYSTEM');

INSERT INTO mst_template_details(TemplateID, LangCode, TemplateTitle, TemplateContent, CreatedDate, CreatedBy)
VALUES((SELECT TemplateID FROM mst_template WHERE TemplateCode='TR02' LIMIT 1), 
'fr', 'Rendez-vous de test programmé', 'Un test programmé est à venir pour l''ID du patient : ###<patient_id>###, le nom du patient : ###<patient_name>###, défini pour ###<time>###. Le médecin traitant est ###<nom_docteur>###.',
NOW(), 'SYSTEM');