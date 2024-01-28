import { FC } from "react";
import classNames from "classnames";
import { AlbumProps } from "../AlbumCard/AlbumCard.component";
import Album from "../AlbumCard";

type AlbumlistProps = {
  title: string;
  wrap?: boolean;
  className?: string;
  albumsData?: Omit<AlbumProps, "className">[];
};

const AlbumList: FC<AlbumlistProps> = ({
  className: classes,
  title,
  albumsData,
  wrap = true,
}) => {
  const className = classNames("album-list", classes);
  return (
    <div className={className}>
      <h2 className="album-list__title">{title}</h2>
      <div
        className={`album-list__albums album-list__albums${
          wrap ? "--wrap" : "--no-wrap"
        }`}
      >
        {albumsData &&
          albumsData.map((album, index) => {
            return <Album className="user-list__users__user" {...album} />;
          })}
      </div>
    </div>
  );
};

export default AlbumList;
