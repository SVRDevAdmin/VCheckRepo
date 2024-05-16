INSERT INTO mst_mastercodedata(CodeGroup, CodeID, CodeName, DESCRIPTION, IsActive, SeqOrder, CreatedDate, CreatedBy)
VALUES('SortDirection', 'DESC', 'Latest', 'Latest record', 1, 1, NOW(), 'ADMIN');

INSERT INTO mst_mastercodedata(CodeGroup, CodeID, CodeName, DESCRIPTION, IsActive, SeqOrder, CreatedDate, CreatedBy)
VALUES('SortDirection', 'ASC', 'Earliest', 'Older record', 1, 2, NOW(), 'ADMIN');