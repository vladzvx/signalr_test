create table Protocols(
    id bigint not null,
    type bigint not null ,
    creation_timestamp timestamp not null,
    patient_id bigint not null,
    header_id bigint default 0,
    parent_id bigint default 0,
    doctor_id bigint,
    data jsonb,
    primary key (id,creation_timestamp)
) partition by range (creation_timestamp);

create table ProtocolsCache(
    id bigserial not null,
    type bigint not null ,
    creation_timestamp timestamp not null,
    patient_id bigint not null,
    header_id bigint default 0,
    parent_id bigint default 0,
    doctor_id bigint,
    data jsonb,
    primary key (id)
);

CREATE TABLE Protocols_2014_m01 PARTITION OF Protocols FOR VALUES FROM ('2014-01-01') TO ('2014-02-01');
CREATE TABLE Protocols_2014_m02 PARTITION OF Protocols FOR VALUES FROM ('2014-02-01') TO ('2014-03-01');
CREATE TABLE Protocols_2014_m03 PARTITION OF Protocols FOR VALUES FROM ('2014-03-01') TO ('2014-04-01');
CREATE TABLE Protocols_2014_m04 PARTITION OF Protocols FOR VALUES FROM ('2014-04-01') TO ('2014-05-01');
CREATE TABLE Protocols_2014_m05 PARTITION OF Protocols FOR VALUES FROM ('2014-05-01') TO ('2014-06-01');
CREATE TABLE Protocols_2014_m06 PARTITION OF Protocols FOR VALUES FROM ('2014-06-01') TO ('2014-07-01');
CREATE TABLE Protocols_2014_m07 PARTITION OF Protocols FOR VALUES FROM ('2014-07-01') TO ('2014-08-01');
CREATE TABLE Protocols_2014_m08 PARTITION OF Protocols FOR VALUES FROM ('2014-08-01') TO ('2014-09-01');
CREATE TABLE Protocols_2014_m09 PARTITION OF Protocols FOR VALUES FROM ('2014-09-01') TO ('2014-10-01');
CREATE TABLE Protocols_2014_m10 PARTITION OF Protocols FOR VALUES FROM ('2014-10-01') TO ('2014-11-01');
CREATE TABLE Protocols_2014_m11 PARTITION OF Protocols FOR VALUES FROM ('2014-11-01') TO ('2014-12-01');
CREATE TABLE Protocols_2014_m12 PARTITION OF Protocols FOR VALUES FROM ('2014-12-01') TO ('2015-01-01');
CREATE TABLE Protocols_2015_m01 PARTITION OF Protocols FOR VALUES FROM ('2015-01-01') TO ('2015-02-01');
CREATE TABLE Protocols_2015_m02 PARTITION OF Protocols FOR VALUES FROM ('2015-02-01') TO ('2015-03-01');
CREATE TABLE Protocols_2015_m03 PARTITION OF Protocols FOR VALUES FROM ('2015-03-01') TO ('2015-04-01');
CREATE TABLE Protocols_2015_m04 PARTITION OF Protocols FOR VALUES FROM ('2015-04-01') TO ('2015-05-01');
CREATE TABLE Protocols_2015_m05 PARTITION OF Protocols FOR VALUES FROM ('2015-05-01') TO ('2015-06-01');
CREATE TABLE Protocols_2015_m06 PARTITION OF Protocols FOR VALUES FROM ('2015-06-01') TO ('2015-07-01');
CREATE TABLE Protocols_2015_m07 PARTITION OF Protocols FOR VALUES FROM ('2015-07-01') TO ('2015-08-01');
CREATE TABLE Protocols_2015_m08 PARTITION OF Protocols FOR VALUES FROM ('2015-08-01') TO ('2015-09-01');
CREATE TABLE Protocols_2015_m09 PARTITION OF Protocols FOR VALUES FROM ('2015-09-01') TO ('2015-10-01');
CREATE TABLE Protocols_2015_m10 PARTITION OF Protocols FOR VALUES FROM ('2015-10-01') TO ('2015-11-01');
CREATE TABLE Protocols_2015_m11 PARTITION OF Protocols FOR VALUES FROM ('2015-11-01') TO ('2015-12-01');
CREATE TABLE Protocols_2015_m12 PARTITION OF Protocols FOR VALUES FROM ('2015-12-01') TO ('2016-01-01');
CREATE TABLE Protocols_2016_m01 PARTITION OF Protocols FOR VALUES FROM ('2016-01-01') TO ('2016-02-01');
CREATE TABLE Protocols_2016_m02 PARTITION OF Protocols FOR VALUES FROM ('2016-02-01') TO ('2016-03-01');
CREATE TABLE Protocols_2016_m03 PARTITION OF Protocols FOR VALUES FROM ('2016-03-01') TO ('2016-04-01');
CREATE TABLE Protocols_2016_m04 PARTITION OF Protocols FOR VALUES FROM ('2016-04-01') TO ('2016-05-01');
CREATE TABLE Protocols_2016_m05 PARTITION OF Protocols FOR VALUES FROM ('2016-05-01') TO ('2016-06-01');
CREATE TABLE Protocols_2016_m06 PARTITION OF Protocols FOR VALUES FROM ('2016-06-01') TO ('2016-07-01');
CREATE TABLE Protocols_2016_m07 PARTITION OF Protocols FOR VALUES FROM ('2016-07-01') TO ('2016-08-01');
CREATE TABLE Protocols_2016_m08 PARTITION OF Protocols FOR VALUES FROM ('2016-08-01') TO ('2016-09-01');
CREATE TABLE Protocols_2016_m09 PARTITION OF Protocols FOR VALUES FROM ('2016-09-01') TO ('2016-10-01');
CREATE TABLE Protocols_2016_m10 PARTITION OF Protocols FOR VALUES FROM ('2016-10-01') TO ('2016-11-01');
CREATE TABLE Protocols_2016_m11 PARTITION OF Protocols FOR VALUES FROM ('2016-11-01') TO ('2016-12-01');
CREATE TABLE Protocols_2016_m12 PARTITION OF Protocols FOR VALUES FROM ('2016-12-01') TO ('2017-01-01');
CREATE TABLE Protocols_2017_m01 PARTITION OF Protocols FOR VALUES FROM ('2017-01-01') TO ('2017-02-01');
CREATE TABLE Protocols_2017_m02 PARTITION OF Protocols FOR VALUES FROM ('2017-02-01') TO ('2017-03-01');
CREATE TABLE Protocols_2017_m03 PARTITION OF Protocols FOR VALUES FROM ('2017-03-01') TO ('2017-04-01');
CREATE TABLE Protocols_2017_m04 PARTITION OF Protocols FOR VALUES FROM ('2017-04-01') TO ('2017-05-01');
CREATE TABLE Protocols_2017_m05 PARTITION OF Protocols FOR VALUES FROM ('2017-05-01') TO ('2017-06-01');
CREATE TABLE Protocols_2017_m06 PARTITION OF Protocols FOR VALUES FROM ('2017-06-01') TO ('2017-07-01');
CREATE TABLE Protocols_2017_m07 PARTITION OF Protocols FOR VALUES FROM ('2017-07-01') TO ('2017-08-01');
CREATE TABLE Protocols_2017_m08 PARTITION OF Protocols FOR VALUES FROM ('2017-08-01') TO ('2017-09-01');
CREATE TABLE Protocols_2017_m09 PARTITION OF Protocols FOR VALUES FROM ('2017-09-01') TO ('2017-10-01');
CREATE TABLE Protocols_2017_m10 PARTITION OF Protocols FOR VALUES FROM ('2017-10-01') TO ('2017-11-01');
CREATE TABLE Protocols_2017_m11 PARTITION OF Protocols FOR VALUES FROM ('2017-11-01') TO ('2017-12-01');
CREATE TABLE Protocols_2017_m12 PARTITION OF Protocols FOR VALUES FROM ('2017-12-01') TO ('2018-01-01');
CREATE TABLE Protocols_2018_m01 PARTITION OF Protocols FOR VALUES FROM ('2018-01-01') TO ('2018-02-01');
CREATE TABLE Protocols_2018_m02 PARTITION OF Protocols FOR VALUES FROM ('2018-02-01') TO ('2018-03-01');
CREATE TABLE Protocols_2018_m03 PARTITION OF Protocols FOR VALUES FROM ('2018-03-01') TO ('2018-04-01');
CREATE TABLE Protocols_2018_m04 PARTITION OF Protocols FOR VALUES FROM ('2018-04-01') TO ('2018-05-01');
CREATE TABLE Protocols_2018_m05 PARTITION OF Protocols FOR VALUES FROM ('2018-05-01') TO ('2018-06-01');
CREATE TABLE Protocols_2018_m06 PARTITION OF Protocols FOR VALUES FROM ('2018-06-01') TO ('2018-07-01');
CREATE TABLE Protocols_2018_m07 PARTITION OF Protocols FOR VALUES FROM ('2018-07-01') TO ('2018-08-01');
CREATE TABLE Protocols_2018_m08 PARTITION OF Protocols FOR VALUES FROM ('2018-08-01') TO ('2018-09-01');
CREATE TABLE Protocols_2018_m09 PARTITION OF Protocols FOR VALUES FROM ('2018-09-01') TO ('2018-10-01');
CREATE TABLE Protocols_2018_m10 PARTITION OF Protocols FOR VALUES FROM ('2018-10-01') TO ('2018-11-01');
CREATE TABLE Protocols_2018_m11 PARTITION OF Protocols FOR VALUES FROM ('2018-11-01') TO ('2018-12-01');
CREATE TABLE Protocols_2018_m12 PARTITION OF Protocols FOR VALUES FROM ('2018-12-01') TO ('2019-01-01');
CREATE TABLE Protocols_2019_m01 PARTITION OF Protocols FOR VALUES FROM ('2019-01-01') TO ('2019-02-01');
CREATE TABLE Protocols_2019_m02 PARTITION OF Protocols FOR VALUES FROM ('2019-02-01') TO ('2019-03-01');
CREATE TABLE Protocols_2019_m03 PARTITION OF Protocols FOR VALUES FROM ('2019-03-01') TO ('2019-04-01');
CREATE TABLE Protocols_2019_m04 PARTITION OF Protocols FOR VALUES FROM ('2019-04-01') TO ('2019-05-01');
CREATE TABLE Protocols_2019_m05 PARTITION OF Protocols FOR VALUES FROM ('2019-05-01') TO ('2019-06-01');
CREATE TABLE Protocols_2019_m06 PARTITION OF Protocols FOR VALUES FROM ('2019-06-01') TO ('2019-07-01');
CREATE TABLE Protocols_2019_m07 PARTITION OF Protocols FOR VALUES FROM ('2019-07-01') TO ('2019-08-01');
CREATE TABLE Protocols_2019_m08 PARTITION OF Protocols FOR VALUES FROM ('2019-08-01') TO ('2019-09-01');
CREATE TABLE Protocols_2019_m09 PARTITION OF Protocols FOR VALUES FROM ('2019-09-01') TO ('2019-10-01');
CREATE TABLE Protocols_2019_m10 PARTITION OF Protocols FOR VALUES FROM ('2019-10-01') TO ('2019-11-01');
CREATE TABLE Protocols_2019_m11 PARTITION OF Protocols FOR VALUES FROM ('2019-11-01') TO ('2019-12-01');
CREATE TABLE Protocols_2019_m12 PARTITION OF Protocols FOR VALUES FROM ('2019-12-01') TO ('2020-01-01');
CREATE TABLE Protocols_2020_m01 PARTITION OF Protocols FOR VALUES FROM ('2020-01-01') TO ('2020-02-01');
CREATE TABLE Protocols_2020_m02 PARTITION OF Protocols FOR VALUES FROM ('2020-02-01') TO ('2020-03-01');
CREATE TABLE Protocols_2020_m03 PARTITION OF Protocols FOR VALUES FROM ('2020-03-01') TO ('2020-04-01');
CREATE TABLE Protocols_2020_m04 PARTITION OF Protocols FOR VALUES FROM ('2020-04-01') TO ('2020-05-01');
CREATE TABLE Protocols_2020_m05 PARTITION OF Protocols FOR VALUES FROM ('2020-05-01') TO ('2020-06-01');
CREATE TABLE Protocols_2020_m06 PARTITION OF Protocols FOR VALUES FROM ('2020-06-01') TO ('2020-07-01');
CREATE TABLE Protocols_2020_m07 PARTITION OF Protocols FOR VALUES FROM ('2020-07-01') TO ('2020-08-01');
CREATE TABLE Protocols_2020_m08 PARTITION OF Protocols FOR VALUES FROM ('2020-08-01') TO ('2020-09-01');
CREATE TABLE Protocols_2020_m09 PARTITION OF Protocols FOR VALUES FROM ('2020-09-01') TO ('2020-10-01');
CREATE TABLE Protocols_2020_m10 PARTITION OF Protocols FOR VALUES FROM ('2020-10-01') TO ('2020-11-01');
CREATE TABLE Protocols_2020_m11 PARTITION OF Protocols FOR VALUES FROM ('2020-11-01') TO ('2020-12-01');
CREATE TABLE Protocols_2020_m12 PARTITION OF Protocols FOR VALUES FROM ('2020-12-01') TO ('2021-01-01');
CREATE TABLE Protocols_2021_m01 PARTITION OF Protocols FOR VALUES FROM ('2021-01-01') TO ('2021-02-01');
CREATE TABLE Protocols_2021_m02 PARTITION OF Protocols FOR VALUES FROM ('2021-02-01') TO ('2021-03-01');
CREATE TABLE Protocols_2021_m03 PARTITION OF Protocols FOR VALUES FROM ('2021-03-01') TO ('2021-04-01');
CREATE TABLE Protocols_2021_m04 PARTITION OF Protocols FOR VALUES FROM ('2021-04-01') TO ('2021-05-01');
CREATE TABLE Protocols_2021_m05 PARTITION OF Protocols FOR VALUES FROM ('2021-05-01') TO ('2021-06-01');
CREATE TABLE Protocols_2021_m06 PARTITION OF Protocols FOR VALUES FROM ('2021-06-01') TO ('2021-07-01');
CREATE TABLE Protocols_2021_m07 PARTITION OF Protocols FOR VALUES FROM ('2021-07-01') TO ('2021-08-01');
CREATE TABLE Protocols_2021_m08 PARTITION OF Protocols FOR VALUES FROM ('2021-08-01') TO ('2021-09-01');
CREATE TABLE Protocols_2021_m09 PARTITION OF Protocols FOR VALUES FROM ('2021-09-01') TO ('2021-10-01');
CREATE TABLE Protocols_2021_m10 PARTITION OF Protocols FOR VALUES FROM ('2021-10-01') TO ('2021-11-01');
CREATE TABLE Protocols_2021_m11 PARTITION OF Protocols FOR VALUES FROM ('2021-11-01') TO ('2021-12-01');
CREATE TABLE Protocols_2021_m12 PARTITION OF Protocols FOR VALUES FROM ('2021-12-01') TO ('2022-01-01');
CREATE TABLE Protocols_2022_m01 PARTITION OF Protocols FOR VALUES FROM ('2022-01-01') TO ('2022-02-01');
CREATE TABLE Protocols_2022_m02 PARTITION OF Protocols FOR VALUES FROM ('2022-02-01') TO ('2022-03-01');
CREATE TABLE Protocols_2022_m03 PARTITION OF Protocols FOR VALUES FROM ('2022-03-01') TO ('2022-04-01');
CREATE TABLE Protocols_2022_m04 PARTITION OF Protocols FOR VALUES FROM ('2022-04-01') TO ('2022-05-01');
CREATE TABLE Protocols_2022_m05 PARTITION OF Protocols FOR VALUES FROM ('2022-05-01') TO ('2022-06-01');
CREATE TABLE Protocols_2022_m06 PARTITION OF Protocols FOR VALUES FROM ('2022-06-01') TO ('2022-07-01');
CREATE TABLE Protocols_2022_m07 PARTITION OF Protocols FOR VALUES FROM ('2022-07-01') TO ('2022-08-01');
CREATE TABLE Protocols_2022_m08 PARTITION OF Protocols FOR VALUES FROM ('2022-08-01') TO ('2022-09-01');
CREATE TABLE Protocols_2022_m09 PARTITION OF Protocols FOR VALUES FROM ('2022-09-01') TO ('2022-10-01');
CREATE TABLE Protocols_2022_m10 PARTITION OF Protocols FOR VALUES FROM ('2022-10-01') TO ('2022-11-01');
CREATE TABLE Protocols_2022_m11 PARTITION OF Protocols FOR VALUES FROM ('2022-11-01') TO ('2022-12-01');
CREATE TABLE Protocols_2022_m12 PARTITION OF Protocols FOR VALUES FROM ('2022-12-01') TO ('2023-01-01');
CREATE TABLE Protocols_2023_m01 PARTITION OF Protocols FOR VALUES FROM ('2023-01-01') TO ('2023-02-01');
CREATE TABLE Protocols_2023_m02 PARTITION OF Protocols FOR VALUES FROM ('2023-02-01') TO ('2023-03-01');
CREATE TABLE Protocols_2023_m03 PARTITION OF Protocols FOR VALUES FROM ('2023-03-01') TO ('2023-04-01');
CREATE TABLE Protocols_2023_m04 PARTITION OF Protocols FOR VALUES FROM ('2023-04-01') TO ('2023-05-01');
CREATE TABLE Protocols_2023_m05 PARTITION OF Protocols FOR VALUES FROM ('2023-05-01') TO ('2023-06-01');
CREATE TABLE Protocols_2023_m06 PARTITION OF Protocols FOR VALUES FROM ('2023-06-01') TO ('2023-07-01');
CREATE TABLE Protocols_2023_m07 PARTITION OF Protocols FOR VALUES FROM ('2023-07-01') TO ('2023-08-01');
CREATE TABLE Protocols_2023_m08 PARTITION OF Protocols FOR VALUES FROM ('2023-08-01') TO ('2023-09-01');
CREATE TABLE Protocols_2023_m09 PARTITION OF Protocols FOR VALUES FROM ('2023-09-01') TO ('2023-10-01');
CREATE TABLE Protocols_2023_m10 PARTITION OF Protocols FOR VALUES FROM ('2023-10-01') TO ('2023-11-01');
CREATE TABLE Protocols_2023_m11 PARTITION OF Protocols FOR VALUES FROM ('2023-11-01') TO ('2023-12-01');
CREATE TABLE Protocols_2023_m12 PARTITION OF Protocols FOR VALUES FROM ('2023-12-01') TO ('2024-01-01');
CREATE TABLE Protocols_2024_m01 PARTITION OF Protocols FOR VALUES FROM ('2024-01-01') TO ('2024-02-01');
CREATE TABLE Protocols_2024_m02 PARTITION OF Protocols FOR VALUES FROM ('2024-02-01') TO ('2024-03-01');
CREATE TABLE Protocols_2024_m03 PARTITION OF Protocols FOR VALUES FROM ('2024-03-01') TO ('2024-04-01');
CREATE TABLE Protocols_2024_m04 PARTITION OF Protocols FOR VALUES FROM ('2024-04-01') TO ('2024-05-01');
CREATE TABLE Protocols_2024_m05 PARTITION OF Protocols FOR VALUES FROM ('2024-05-01') TO ('2024-06-01');
CREATE TABLE Protocols_2024_m06 PARTITION OF Protocols FOR VALUES FROM ('2024-06-01') TO ('2024-07-01');
CREATE TABLE Protocols_2024_m07 PARTITION OF Protocols FOR VALUES FROM ('2024-07-01') TO ('2024-08-01');
CREATE TABLE Protocols_2024_m08 PARTITION OF Protocols FOR VALUES FROM ('2024-08-01') TO ('2024-09-01');
CREATE TABLE Protocols_2024_m09 PARTITION OF Protocols FOR VALUES FROM ('2024-09-01') TO ('2024-10-01');
CREATE TABLE Protocols_2024_m10 PARTITION OF Protocols FOR VALUES FROM ('2024-10-01') TO ('2024-11-01');
CREATE TABLE Protocols_2024_m11 PARTITION OF Protocols FOR VALUES FROM ('2024-11-01') TO ('2024-12-01');
CREATE TABLE Protocols_2024_m12 PARTITION OF Protocols FOR VALUES FROM ('2024-12-01') TO ('2025-01-01');
CREATE TABLE Protocols_2025_m01 PARTITION OF Protocols FOR VALUES FROM ('2025-01-01') TO ('2025-02-01');
CREATE TABLE Protocols_2025_m02 PARTITION OF Protocols FOR VALUES FROM ('2025-02-01') TO ('2025-03-01');
CREATE TABLE Protocols_2025_m03 PARTITION OF Protocols FOR VALUES FROM ('2025-03-01') TO ('2025-04-01');
CREATE TABLE Protocols_2025_m04 PARTITION OF Protocols FOR VALUES FROM ('2025-04-01') TO ('2025-05-01');
CREATE TABLE Protocols_2025_m05 PARTITION OF Protocols FOR VALUES FROM ('2025-05-01') TO ('2025-06-01');
CREATE TABLE Protocols_2025_m06 PARTITION OF Protocols FOR VALUES FROM ('2025-06-01') TO ('2025-07-01');
CREATE TABLE Protocols_2025_m07 PARTITION OF Protocols FOR VALUES FROM ('2025-07-01') TO ('2025-08-01');
CREATE TABLE Protocols_2025_m08 PARTITION OF Protocols FOR VALUES FROM ('2025-08-01') TO ('2025-09-01');
CREATE TABLE Protocols_2025_m09 PARTITION OF Protocols FOR VALUES FROM ('2025-09-01') TO ('2025-10-01');
CREATE TABLE Protocols_2025_m10 PARTITION OF Protocols FOR VALUES FROM ('2025-10-01') TO ('2025-11-01');
CREATE TABLE Protocols_2025_m11 PARTITION OF Protocols FOR VALUES FROM ('2025-11-01') TO ('2025-12-01');
CREATE TABLE Protocols_2025_m12 PARTITION OF Protocols FOR VALUES FROM ('2025-12-01') TO ('2026-01-01');
CREATE TABLE Protocols_2026_m01 PARTITION OF Protocols FOR VALUES FROM ('2026-01-01') TO ('2026-02-01');
CREATE TABLE Protocols_2026_m02 PARTITION OF Protocols FOR VALUES FROM ('2026-02-01') TO ('2026-03-01');
CREATE TABLE Protocols_2026_m03 PARTITION OF Protocols FOR VALUES FROM ('2026-03-01') TO ('2026-04-01');
CREATE TABLE Protocols_2026_m04 PARTITION OF Protocols FOR VALUES FROM ('2026-04-01') TO ('2026-05-01');
CREATE TABLE Protocols_2026_m05 PARTITION OF Protocols FOR VALUES FROM ('2026-05-01') TO ('2026-06-01');
CREATE TABLE Protocols_2026_m06 PARTITION OF Protocols FOR VALUES FROM ('2026-06-01') TO ('2026-07-01');
CREATE TABLE Protocols_2026_m07 PARTITION OF Protocols FOR VALUES FROM ('2026-07-01') TO ('2026-08-01');
CREATE TABLE Protocols_2026_m08 PARTITION OF Protocols FOR VALUES FROM ('2026-08-01') TO ('2026-09-01');
CREATE TABLE Protocols_2026_m09 PARTITION OF Protocols FOR VALUES FROM ('2026-09-01') TO ('2026-10-01');
CREATE TABLE Protocols_2026_m10 PARTITION OF Protocols FOR VALUES FROM ('2026-10-01') TO ('2026-11-01');
CREATE TABLE Protocols_2026_m11 PARTITION OF Protocols FOR VALUES FROM ('2026-11-01') TO ('2026-12-01');
CREATE TABLE Protocols_2026_m12 PARTITION OF Protocols FOR VALUES FROM ('2026-12-01') TO ('2027-01-01');
CREATE TABLE Protocols_2027_m01 PARTITION OF Protocols FOR VALUES FROM ('2027-01-01') TO ('2027-02-01');
CREATE TABLE Protocols_2027_m02 PARTITION OF Protocols FOR VALUES FROM ('2027-02-01') TO ('2027-03-01');
CREATE TABLE Protocols_2027_m03 PARTITION OF Protocols FOR VALUES FROM ('2027-03-01') TO ('2027-04-01');
CREATE TABLE Protocols_2027_m04 PARTITION OF Protocols FOR VALUES FROM ('2027-04-01') TO ('2027-05-01');
CREATE TABLE Protocols_2027_m05 PARTITION OF Protocols FOR VALUES FROM ('2027-05-01') TO ('2027-06-01');
CREATE TABLE Protocols_2027_m06 PARTITION OF Protocols FOR VALUES FROM ('2027-06-01') TO ('2027-07-01');
CREATE TABLE Protocols_2027_m07 PARTITION OF Protocols FOR VALUES FROM ('2027-07-01') TO ('2027-08-01');
CREATE TABLE Protocols_2027_m08 PARTITION OF Protocols FOR VALUES FROM ('2027-08-01') TO ('2027-09-01');
CREATE TABLE Protocols_2027_m09 PARTITION OF Protocols FOR VALUES FROM ('2027-09-01') TO ('2027-10-01');
CREATE TABLE Protocols_2027_m10 PARTITION OF Protocols FOR VALUES FROM ('2027-10-01') TO ('2027-11-01');
CREATE TABLE Protocols_2027_m11 PARTITION OF Protocols FOR VALUES FROM ('2027-11-01') TO ('2027-12-01');
CREATE TABLE Protocols_2027_m12 PARTITION OF Protocols FOR VALUES FROM ('2027-12-01') TO ('2028-01-01');
CREATE TABLE Protocols_2028_m01 PARTITION OF Protocols FOR VALUES FROM ('2028-01-01') TO ('2028-02-01');
CREATE TABLE Protocols_2028_m02 PARTITION OF Protocols FOR VALUES FROM ('2028-02-01') TO ('2028-03-01');
CREATE TABLE Protocols_2028_m03 PARTITION OF Protocols FOR VALUES FROM ('2028-03-01') TO ('2028-04-01');
CREATE TABLE Protocols_2028_m04 PARTITION OF Protocols FOR VALUES FROM ('2028-04-01') TO ('2028-05-01');
CREATE TABLE Protocols_2028_m05 PARTITION OF Protocols FOR VALUES FROM ('2028-05-01') TO ('2028-06-01');
CREATE TABLE Protocols_2028_m06 PARTITION OF Protocols FOR VALUES FROM ('2028-06-01') TO ('2028-07-01');
CREATE TABLE Protocols_2028_m07 PARTITION OF Protocols FOR VALUES FROM ('2028-07-01') TO ('2028-08-01');
CREATE TABLE Protocols_2028_m08 PARTITION OF Protocols FOR VALUES FROM ('2028-08-01') TO ('2028-09-01');
CREATE TABLE Protocols_2028_m09 PARTITION OF Protocols FOR VALUES FROM ('2028-09-01') TO ('2028-10-01');
CREATE TABLE Protocols_2028_m10 PARTITION OF Protocols FOR VALUES FROM ('2028-10-01') TO ('2028-11-01');
CREATE TABLE Protocols_2028_m11 PARTITION OF Protocols FOR VALUES FROM ('2028-11-01') TO ('2028-12-01');
CREATE TABLE Protocols_2028_m12 PARTITION OF Protocols FOR VALUES FROM ('2028-12-01') TO ('2029-01-01');
CREATE TABLE Protocols_2029_m01 PARTITION OF Protocols FOR VALUES FROM ('2029-01-01') TO ('2029-02-01');
CREATE TABLE Protocols_2029_m02 PARTITION OF Protocols FOR VALUES FROM ('2029-02-01') TO ('2029-03-01');
CREATE TABLE Protocols_2029_m03 PARTITION OF Protocols FOR VALUES FROM ('2029-03-01') TO ('2029-04-01');
CREATE TABLE Protocols_2029_m04 PARTITION OF Protocols FOR VALUES FROM ('2029-04-01') TO ('2029-05-01');
CREATE TABLE Protocols_2029_m05 PARTITION OF Protocols FOR VALUES FROM ('2029-05-01') TO ('2029-06-01');
CREATE TABLE Protocols_2029_m06 PARTITION OF Protocols FOR VALUES FROM ('2029-06-01') TO ('2029-07-01');
CREATE TABLE Protocols_2029_m07 PARTITION OF Protocols FOR VALUES FROM ('2029-07-01') TO ('2029-08-01');
CREATE TABLE Protocols_2029_m08 PARTITION OF Protocols FOR VALUES FROM ('2029-08-01') TO ('2029-09-01');
CREATE TABLE Protocols_2029_m09 PARTITION OF Protocols FOR VALUES FROM ('2029-09-01') TO ('2029-10-01');
CREATE TABLE Protocols_2029_m10 PARTITION OF Protocols FOR VALUES FROM ('2029-10-01') TO ('2029-11-01');
CREATE TABLE Protocols_2029_m11 PARTITION OF Protocols FOR VALUES FROM ('2029-11-01') TO ('2029-12-01');
CREATE TABLE Protocols_2029_m12 PARTITION OF Protocols FOR VALUES FROM ('2029-12-01') TO ('2030-01-01');

create index Protocols_patient_index on Protocols using hash(patient_id);
create index Protocols_doctor_index on Protocols using hash(doctor_id);
create index Protocols_id_index on Protocols using hash(id);
create index Protocols_header_id_index on Protocols using hash(header_id);


create index ProtocolsCache_patient_index on ProtocolsCache using hash(patient_id);
create index ProtocolsCache_doctor_index on ProtocolsCache using hash(doctor_id);
create index ProtocolsCache_id_index on ProtocolsCache using hash(id);
create index ProtocolsCache_header_id_index on ProtocolsCache using hash(header_id);


CREATE OR REPLACE FUNCTION add_protocol(_id bigint, _type bigint,_creation_timestamp timestamp,_patient_id bigint,
    _header_id bigint ,_parent_id bigint, _doctor_id bigint,_data jsonb) RETURNS void as
$$
    begin
        _data=_data-'{patient,id,parent,header,_id}'::text[];
        insert into ProtocolsCache (id, type, creation_timestamp, data,patient_id,header_id,parent_id,doctor_id) values
                             (_id, _type,_creation_timestamp,_data,_patient_id,_header_id,_parent_id,_doctor_id) ON CONFLICT ON CONSTRAINT protocolscache_pkey
        do update SET type=_type, data=_data where ProtocolsCache.id=EXCLUDED.id;
    end;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION write_protocol_to_history(_id bigint, _type bigint,_creation_timestamp timestamp,_patient_id bigint,
    _header_id bigint ,_parent_id bigint, _data jsonb) RETURNS void as
$$
    begin
        _data=_data-'{patient,id,parent,header,_id}'::text[];
        insert into Protocols (id, type, creation_timestamp, data,patient_id,header_id,parent_id) values
                             (_id, _type,_creation_timestamp,_data,_patient_id,_header_id,_parent_id);
    end;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION rotate_protocols(limit_time timestamp) RETURNS void as
$$
    begin
        with temp as(
            select id, type, creation_timestamp, patient_id, header_id, parent_id, data from protocolscache where creation_timestamp<limit_time
        )
        insert into Protocols select id, type, creation_timestamp, patient_id, header_id, parent_id, data from temp on conflict ON CONSTRAINT protocols_pkey do update
            SET type=EXCLUDED.type,
                data=EXCLUDED.data
        where Protocols.id=EXCLUDED.id;
        delete from protocolscache where creation_timestamp<limit_time;
    end;
$$ LANGUAGE plpgsql;


insert into ProtocolsCache (id, type, creation_timestamp, data, patient_id) values
                 (260, 11,'2021-08-16 12:13:59.137868','{"time": "2021-08-13T12:13:58.7364369Z", "values": {"FreeText": ["test"]}}'::jsonb,1) ON CONFLICT ON CONSTRAINT protocolscache_pkey
do update SET type=11, data='{"time": "2021-08-13T12:13:58.7364369Z", "values": {"FreeText": ["test4"]}}'::jsonb where ProtocolsCache.id=EXCLUDED.id;




create table if not exists groups(
    update_time timestamp default current_timestamp not null,
    id bigserial,
    name text,
    last_message_id bigint default 0,
    primary key (id)
);
insert into groups (name, id) values ('system',0) on conflict on constraint groups_pkey do nothing ;
create table  if not exists  users(
    update_time timestamp default current_timestamp not null,
    id bigint,
    name text,
    primary key (id)
);

create table if not exists users_groups(
    id bigserial,
    user_id bigint,
    group_id bigint,
    time timestamp default current_timestamp not null,
    primary key (id),
    foreign key (user_id) references users (id),
    foreign key (group_id) references groups(id)
);

CREATE table if not exists data_table(
    id bigserial,
    time timestamp not null,
    group_id bigint,
    message_id bigint,
    user_id bigint,
    data jsonb,
    primary key (time,group_id,message_id),
    foreign key (group_id) references groups (id),
    foreign key (user_id) references users (id)
);

create table if not exists acknowledgments(
    time timestamp not null default current_timestamp,
    id bigserial,
    group_id bigint,
    user_id bigint,
    message_id bigint,
    foreign key (user_id) references users (id),
    foreign key (group_id) references groups (id),
    primary key (time,id)
);

CREATE OR REPLACE FUNCTION write_message(_time timestamp, _group_id bigint,_message_id bigint,_user_id bigint,_data jsonb) RETURNS void as
$$
    begin
        if (_data->'MessageType')::integer = 2 then
                    insert into acknowledgments ( group_id,message_id,user_id) values
                             (_group_id,_message_id,_user_id) ON CONFLICT ON CONSTRAINT acknowledgments_pkey do nothing;
            else
                    insert into data_table (time, group_id,message_id,user_id,data) values
                             (_time, _group_id,_message_id,_user_id,_data) ON CONFLICT ON CONSTRAINT data_table_pkey
                    do update SET data=_data where data_table.group_id=EXCLUDED.group_id and data_table.message_id=EXCLUDED.message_id;
        end if;

    end;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION read_history(_group_id bigint,_message_id bigint) RETURNS table(data2 jsonb) as
$$
    begin
        return query select data from data_table where group_id = @_group_id and message_id > @_message_id;

    end;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION last_message_update() RETURNS trigger as
$$
    begin
        update groups set last_message_id=new.message_id where group_id=new.group_id;
    end;
$$ LANGUAGE plpgsql;

drop trigger if exists on_insert_to_data_table_ancets on data_table_ancets;
CREATE TRIGGER on_insert_to_data_table after INSERT on data_table FOR EACH ROW execute PROCEDURE last_message_update();

select write_message_ancets('2021-08-25 11:53:27.828660', 0, 1, 71790902, '{}');

 insert into data_table_ancets (time, group_id,message_id,user_id) values
                             ('2021-08-25 11:53:27.828660',0,0,71790902) ON CONFLICT ON CONSTRAINT data_table_Ancets_pkey do nothing;