create table json_storage(
    id bigserial not null ,
    support_id bigint default 0,
    time timestamp default current_timestamp,
    data jsonb,
    primary key (id,support_id)
) partition by range (support_id);

CREATE TABLE json_storage_middle PARTITION OF json_storage FOR VALUES FROM (0) TO (10);

create index json_storage_id ON json_storage using hash(id);

CREATE OR REPLACE FUNCTION write_json(_id bigint, _data jsonb) RETURNS void as
$$
    begin
        if (exists(select 1 from json_storage where id=_id)) then
                update json_storage set data=_data where id=_id;
            else
                insert into json_storage (id, data) values (_id, _data);
        end if;

    end;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION read_json(_id bigint) RETURNS jsonb as
$$
    begin
        return (select data from json_storage where id=_id);
    end;
$$ LANGUAGE plpgsql;