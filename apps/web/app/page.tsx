import Link from 'next/link';

export default function HomePage() {
  return (
    <main className="shell">
      <section className="card stack">
        <p className="muted">Overseas Chinese Community + Direct Recruitment</p>
        <h1>NeighbourHire</h1>
        <p>连接本地社区、可信组织与真实工作机会。</p>
        <div className="row">
          <Link className="button" href="/login">登录</Link>
          <Link className="button secondary" href="/profile">个人资料</Link>
        </div>
      </section>
    </main>
  );
}
