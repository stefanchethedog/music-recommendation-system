import { FC } from "react";
import classNames from "classnames";
import { GenreProps } from "../GenreCard/GenreCard.component";
import Genre from "../GenreCard";

import './GenresList.styles.scss';

type GenreslistProps = {
  title: string;
  wrap?: boolean;
  className?: string;
  genresData?: Omit<GenreProps, "className">[];
};

const GenresList: FC<GenreslistProps> = ({
  className: classes,
  title,
  genresData,
  wrap = true,
}) => {
  const className = classNames("genres-list", classes);
  return (
    <div className={className}>
      <h2 className="genres-list__title">{title}</h2>
      <div
        className={`genres-list__genres genres-list__genres${
          wrap ? "--wrap" : "--no-wrap"
        }`}
      >
        {genresData &&
          genresData.map((genre, index) => {
            return <Genre className="Genres-list__Genress__Genres" {...genre} />;
          })}
      </div>
    </div>
  );
};

export default GenresList;
