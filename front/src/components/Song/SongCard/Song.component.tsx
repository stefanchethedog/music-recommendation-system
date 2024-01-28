import { FC } from "react";
import classNames from "classnames";

import "./Song.styles.scss";

export type SongProps = {
  name: String;
  genres: String[];
  author: String;
  className?: String;
};

const Song: FC<SongProps> = ({ className, name, genres, author }) => {
  const classes = classNames("song__container", className);

  return (
    <div className={classes}>
      <div className="song__container__title">{name}</div>
      <div className="song__container__author">{author}</div>
      <div className="song__container__genres">
        {genres.map((genre) => (
          <div className="song__container__genres__genre">{genre},</div>
        ))}
      </div>
    </div>
  );
};

export default Song;
