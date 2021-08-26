using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IASK.DataStorage.Interfaces
{
    public interface IDbCreator<T>
    {
        public static string Name
        {
            get
            {
                Regex reg = new Regex(@".+\.(\w+$)");
                string temp = typeof(T).ToString();
                var t = reg.Match(temp);
                temp = t.Groups[1].Value;
                if (!temp.Equals("Message"))
                {
                    return temp.Replace("Message", "");
                }
                else return temp;
            }
        }
        public async Task CreateDB(DbConnection dbConnection)
        {
            DbCommand command = dbConnection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = string.Format(
            "create table if not exists groups_{0} (\n" +
            "    update_time timestamp default current_timestamp not null,\n" +
            "    id bigserial,\n" +
            "    name text,\n" +
            "    last_message_id bigint default 0,\n" +
            "    primary key (id)\n" +
            ");\n" +
            "insert into groups_{0} (name, id) values ('system',0) on conflict on constraint groups_{0}_pkey do nothing ;\n" +
            "create table  if not exists  users_{0}(\n" +
            "    update_time timestamp default current_timestamp not null,\n" +
            "    id bigint,\n" +
            "    name text,\n" +
            "    primary key (id)\n" +
            ");\n" +

            "create table if not exists users_groups_{0}(\n" +
            "    id bigserial,\n" +
            "    user_id bigint,\n" +
            "    group_id bigint,\n" +
            "    time timestamp default current_timestamp not null,\n" +
            "    primary key (id),\n" +
            "    foreign key (user_id) references users_{0} (id),\n" +
            "    foreign key (group_id) references groups_{0}(id)\n" +
            ");\n" +
            "CREATE table if not exists data_table_{0}(\n" +
            "    id bigserial,\n" +
            "    time timestamp not null,\n" +
            "    group_id bigint,\n" +
            "    message_id bigint,\n" +
            "    user_id bigint,\n" +
            "    data jsonb,\n" +
            "    primary key (time,group_id,message_id),\n" +
            "    foreign key (group_id) references groups_{0} (id),\n" +
            "    foreign key (user_id) references users_{0} (id)\n" +
            ");\n" +

            "create table if not exists acknowledgments_{0}(\n" +
            "    time timestamp not null default current_timestamp,\n" +
            "    id bigserial,\n" +
            "    group_id bigint,\n" +
            "    user_id bigint,\n" +
            "    message_id bigint,\n" +
            "    foreign key (user_id) references users_{0} (id),\n" +
            "    foreign key (group_id) references groups_{0} (id),\n" +
            "    primary key (time,id)\n" +
            ");\n" +

            "CREATE OR REPLACE FUNCTION write_message_{0}(_time timestamp, _group_id bigint,_message_id bigint,_user_id bigint,_data jsonb) RETURNS void as\n" +
            "$$\n" +
            "    begin\n" +
            "        if (_data->'MessageType')::integer = 2 then\n" +
            "                    insert into acknowledgments_{0} ( group_id,message_id,user_id) values\n" +
            "                             (_group_id,_message_id,_user_id) ON CONFLICT ON CONSTRAINT acknowledgments_{0}_pkey do nothing;\n" +
            "            else\n" +
            "                    insert into data_table_{0} (time, group_id,message_id,user_id,data) values\n" +
            "                             (_time, _group_id,_message_id,_user_id,_data) ON CONFLICT ON CONSTRAINT data_table_{0}_pkey\n" +
            "                   do update SET data=_data where data_table_{0}.group_id=EXCLUDED.group_id and data_table_{0}.message_id=EXCLUDED.message_id;\n" +
            "        end if;\n" +
            "    end;\n" +
            "$$ LANGUAGE plpgsql;\n" +

            "CREATE OR REPLACE FUNCTION read_history_{0}(_group_id bigint,_message_id bigint) RETURNS table(data2 jsonb) as\n" +
            "$$\n" +
            "    begin\n" +
            "        return query select data from data_table_{0} where group_id = @_group_id and message_id > @_message_id;\n" +
            "    end;\n" +
            "$$ LANGUAGE plpgsql;\n" +


            "CREATE OR REPLACE FUNCTION last_message_update_{0}() RETURNS trigger as\n" +
            "$$\n" +
            "    begin\n" +
            "        update groups_{0} set last_message_id=new.message_id where id=new.group_id;\n" +
            "return null;\n" +
            
            "    end;\n" +
            "$$ LANGUAGE plpgsql;\n" +

            "drop trigger if exists on_insert_to_data_table_{0} on data_table_{0};\n" +
            "CREATE TRIGGER on_insert_to_data_table_{0} after INSERT on data_table_{0} FOR EACH ROW execute PROCEDURE last_message_update_{0}();"
            , Name);

            await command.ExecuteNonQueryAsync();
        }
    }
}
