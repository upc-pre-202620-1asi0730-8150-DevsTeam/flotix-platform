-- Gives demo data to the accounts that were created BEFORE the demo data feature existed
-- (new accounts get it automatically at sign-up). Safe to run more than once.
-- Requires the "AddDemoDataFunctions" migration (applied when the API starts).
SELECT public.flotix_seed_demo_data(id) FROM users ORDER BY id;
