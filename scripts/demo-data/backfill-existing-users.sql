-- Generates the demo data (about 90 days / 3 months of history up to now) of every account that does not have it yet.
-- The AddDemoDataFunctions migration already runs this once when it is applied, and new accounts get it at sign-up.
-- Safe to run more than once. Requires the "AddDemoDataFunctions" migration (applied when the API starts).
-- To use another history length: SELECT public.flotix_seed_demo_data_for_all_users(<days>);
SELECT public.flotix_seed_demo_data_for_all_users(90) AS seeded_accounts;