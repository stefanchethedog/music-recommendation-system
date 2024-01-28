import { FC } from "react";
import classNames from "classnames";

import "./AlbumCard.styles.scss";

export type AlbumProps = {
  name: String;
  artistName: string;
  songs: string[];
  genres: string[];
  className?: String;
};

const AlbumCard: FC<AlbumProps> = ({
  className: classes,
  name,
  artistName,
  songs,
  genres,
}) => {
  const className = classNames("album", classes);
  return (
    <div className={className}>
      <div className="album__name">{name}</div>
      <div className="album__artist">{artistName}</div>
      <div className="album__songs">
        {songs.map((song, index) => (
          <div className="album__songs__song" id={`${song}${index}`}>
            {song}
          </div>
        ))}
      </div>
      <div className="album__genres">
        {genres.map((genre, index) => (
          <div className="album__genres__genre" id={`${genre}${index}`}>
            {genre}
          </div>
        ))}
      </div>
    </div>
  );
};

export default AlbumCard;
