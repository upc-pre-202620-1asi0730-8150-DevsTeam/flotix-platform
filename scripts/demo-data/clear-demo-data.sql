-- Removes the demo data of ALL accounts (only rows registered in public.demo_data_rows; real data is untouched,
-- except rows that users attached to demo vehicles, which are removed with them).
-- To remove it for a single account use:  SELECT public.flotix_clear_demo_data(<users.id>);
SELECT public.flotix_clear_demo_data(user_id) AS removed_rows
FROM (SELECT DISTINCT user_id FROM public.demo_data_rows) AS accounts
ORDER BY user_id;