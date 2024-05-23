INSERT INTO mst_mastercodedata(CodeGroup, CodeID, CodeName, DESCRIPTION, IsActive, SeqOrder, CreatedDate, CreatedBy)
VALUES('SortDirection', 'DESC', 'Latest', 'Latest record', 1, 1, NOW(), 'ADMIN');

INSERT INTO mst_mastercodedata(CodeGroup, CodeID, CodeName, DESCRIPTION, IsActive, SeqOrder, CreatedDate, CreatedBy)
VALUES('SortDirection', 'ASC', 'Earliest', 'Older record', 1, 2, NOW(), 'ADMIN');

INSERT INTO mst_mastercodedata(CodeGroup, CodeID, CodeName, DESCRIPTION, IsActive, SeqOrder, CreatedDate, CreatedBy)
VALUES('SortDirection', 'Status ASC', 'Result (Ascending)', 'Test Result status Ascending', 1, 3, NOW(), 'ADMIN');

INSERT INTO mst_mastercodedata(CodeGroup, CodeID, CodeName, DESCRIPTION, IsActive, SeqOrder, CreatedDate, CreatedBy)
VALUES('SortDirection', 'Status DESC', 'Result (Descending)', 'Test Result status Descending', 1, 4, NOW(), 'ADMIN');