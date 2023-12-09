import { FC } from "react";
import { useGetDataSourcesQuery } from "./DataSourcesApiSlice";

const DataSources: FC = () => {
    const { data } = useGetDataSourcesQuery();

    return (
        <div>
            <span>DataSources!</span>
            <ul>
                {data?.results.map(({ name }) => (
                    <li>
                        <a href="#">
                            <div>{name}</div>
                        </a>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default DataSources;