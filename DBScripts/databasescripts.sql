CREATE TABLE IF NOT EXISTS public.TestEmployee
(
    id bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    empname character varying(250) COLLATE pg_catalog."default",
    empdept character varying(250) COLLATE pg_catalog."default",
    empsalary money NULL,
    empdob timestamp without time zone,    
    CONSTRAINT beeline_cdf_audit_pkey PRIMARY KEY (id)
)


-------------------------------------------------------------------------------------------------------------------------

CREATE OR REPLACE PROCEDURE public.sp_insert_employee(
	p_empname character varying,
	p_empdept character varying,
	p_empsalary decimal,
	p_empdob timestamp)
LANGUAGE 'plpgsql'
AS $BODY$
BEGIN
	INSERT INTO public.TestEmployee
	(
		empname,
		empdept,
		empsalary, 
		empdob
	)
	VALUES 
	(
		p_empname,
		p_empdept,
		p_empsalary, 
		p_empdob
	);
end;
$BODY$;
------------------------------------------------------------------------------------------------------------------