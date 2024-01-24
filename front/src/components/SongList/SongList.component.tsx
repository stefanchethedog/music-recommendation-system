import { FC } from "react";
import classNames from "classnames";

import "./SongList.styles.scss";
import Song, { SongProps } from "../Song/Song.component";

type SongListProps = {
  className?: String;
  wrap?: Boolean;
  title: String;
  songData: Omit<SongProps, "className">[];
};

const SongList: FC<SongListProps> = ({
  className,
  songData,
  title,
  wrap = true,
}) => {
  const classes = classNames("song-list__container", className);

  return (
    <div className={classes}>
      <h2 className="song-list__container__title">{title}</h2>
      <div
        className={`song-list__container__songs song-list__container__songs${
          wrap ? "--wrap" : "--no-wrap"
        }`}
      >
        {songData.map((song, index) => {
          return (
            <Song className="song-list__container__songs__song" {...song} />
          );
        })}
      </div>
    </div>
  );
};

export default SongList;
